import re
import requests
import json
import logging

action = {
    "block":"deny",
    "allow":"allow"
}
class Firewall:

    def __init__(self,url,arheader,cfkey):
        self.country = []
        self.uri     = []
        self.ip      = []
        self.action  = ""
        self.desc    = ""
        self.arvan_url = url 
        self.arheader  = arheader
        self.cfkey  = cfkey

    def cloudmsg(self,response):
        return ''.join(["Cloudflare: ", response])
    def arvanmsg(self,response):
        return ''.join(["Arvancloud: ", response])

    def expression_parser(self,str):
        logging.debug("Analyzing the phrase firewall rule")
        try:
            self.country = re.findall('ip.geoip.country eq "(.+?)"', str)
            self.uri = re.findall('http.request.uri eq "(.+?)"', str) 
            self.ip = re.findall('ip.src eq (.+?)and', str)
            logging.info("The firewall rule phrase analysis was performed successfully")
        except re.error:
            logging.error("An error occurred while parsing the phrase firewall rule")

    
    def get_firewall_rules(self,zone_id):
        logging.debug(self.cloudmsg("Receiving firewall rules"))
        url = "https://api.cloudflare.com/client/v4/zones/"+zone_id+"/firewall/rules"
        payload={}
        headers = {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer '+self.cfkey
        }
        response = requests.request("GET", url, headers=headers, data=payload).json()
        logging.info(self.cloudmsg("Firewall rules received"))
        self.response_parser(response)

    def response_parser(self,response):
        logging.debug("Analyze the received response of the clodflare")
        for numrule in range(0,len(response["result"])):
            logging.debug("A firewall rule was found")
            self.action = action[response["result"][numrule]["action"]]
            self.desc   = response["result"][numrule]["description"]
            expresion   = response["result"][numrule]["filter"]["expression"]
            self.expression_parser(expresion)
            self.send_firewall_rules()

    def send_firewall_rules(self):
        url = self.arvan_url
        payload={
            "name"          : self.desc ,
            "url_pattern"   : self.uri[0] if len(self.uri) > 0 else "**"  ,
            "sources"       : self.country + self.ip ,
            "action"        : self.action
        }
        print(json.dumps(payload,5))
        response = requests.request("POST", url, headers=self.arheader, data=json.dumps(payload,5))
        if response.status_code == 201 :
            logging.info(self.arvanmsg("This firewall rule was successfully created"))
        else:
            logging.error(self.arvanmsg("An error occurred while registering the firewall rule - {} {} {}".format(self.uri[0],self.action,self.desc)))

