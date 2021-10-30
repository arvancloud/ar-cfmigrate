import requests
import json
import logging
import CloudFlare
from firewall import Firewall 
from color import color
from custom_log import SpecialFormatter

hdlr = logging.StreamHandler()
hdlr.setFormatter(SpecialFormatter())
logging.root.addHandler(hdlr)

class ArvanClient:
    
    def __init__(self,arkey,cfkey,level=10):
        logging.root.setLevel(level)
        self.cf     = CloudFlare.CloudFlare()
        self.cfkey  = cfkey
        self.cf     = CloudFlare.CloudFlare(token=cfkey)
        self.arvancloud_base_url = "https://napi.arvancloud.com/cdn/4.0/"
        self.arvancloud_request_headers = {'Authorization': arkey, 'Content-Type': 'application/json'}
        self.domains    = {}
        self.domais_id  = {}
        self.dns_value  = {
            "A": '[{"ip":"{}"}]',
            "AAAA": '[{"ip":"{}"}]',
            "CNAME": '{"host": "{}", "host_header": "source"}',
            "ANAME": '{"location": "{}", "host_header": "source"}',
            "MX": '{"host": "{}", "priority"}',
            "TXT": '{"text": "{}"}',
            "PTR": '{"domain": "{}"}',
            "NS": '{"host": "{}"}',
            "SRV": '{"target": "{}"}',
            "SPF": '{"text": "{}"}',
            "DKIM": '{"text": "{}"}',
        }

    def msg(self,badge, info):
        print(''.join([color.CYAN, '[', badge, '] ', info, color.END]))

    def cloudmsg(self,response):
        return ''.join(["Cloudflare: ", response])

    def arvanmsg(self,response):
        return ''.join(["Arvancloud: ", response])

    def fetch(self,base, path, head):
        try:
            req = requests.Session()
            req.max_redirects = 60
            r = req.get(''.join([base, path]), headers=head)
            return r
        except requests.exceptions.HTTPError as errh:
            print ("Http Error:", errh)
        except requests.exceptions.ConnectionError as errc:
            print ("Error Connecting:", errc)
        except requests.exceptions.Timeout as errt:
            print ("Timeout Error:", errt)
        except requests.exceptions.RequestException as err:
            print ("OOps: Something Else", err)

    def post(self,path, payload):
        r = requests.post(''.join([self.arvancloud_base_url, path]), json=payload,
                          headers=self.arvancloud_request_headers, allow_redirects=False)
        if r.status_code == 302:
            return r.status_code
        else:
            return r

    def check_cloudflare_apikey(self):
        logging.debug("Cloudflare: Verify Token")
        try:
            logging.info(self.cloudmsg("This API Token is valid"))
        except CloudFlare.exceptions.CloudFlareAPIError as err:
            exit(logging.error(self.cloudmsg("Invalid API Token")))

    def check_arvancloud_apikey(self):
        logging.debug("Arvancloud: Verify Token")
        response = self.fetch(self.arvancloud_base_url, "domains",
                        self.arvancloud_request_headers)
        message = response.json()["message"]
        if message == "Unauthenticated.":
            exit(logging.error(self.arvanmsg("Invalid API Token")))
        else:
            logging.info(self.arvanmsg("This API Token is valid"))

    def get_domains(self):
        logging.debug(self.cloudmsg("Getting a list of domains"))
        zones = self.cf.zones.get()
        for zone in zones:
            zone_id = zone['id']
            zone_name = zone['name']
            self.domais_id[zone_name] = zone_id
            self.domains[zone_name] = zone_id
        logging.info(self.cloudmsg("Received domain list successfully"))
        self.domain_register(self.domains)

    def get_dns_recorde(self,zone_id, domain):
        try:
            dns_records = self.cf.zones.dns_records.get(zone_id)
        except CloudFlare.exceptions.CloudFlareAPIError as e:
            exit('/zones/dns_records.get %d %s - api call failed' % (e, e))
        for dns_record in dns_records:
            attr = {}
            attr["name"] = dns_record['name']
            attr["type"] = dns_record['type']
            attr["value"] = dns_record['content']
            attr["ttl"] = 120
            self.set_dnsrecord_arvan(domain, {"type": attr["type"], "name": attr["name"], "value": json.loads(
                self.dns_value[attr["type"]].replace("{}", attr["value"])), "ttl": attr["ttl"]})

    def set_dnsrecord_arvan(self,domain, attr={}):
        logging.debug(self.arvanmsg("Setting dns domain"))
        response = self.post("domains/{}/dns-records".format(domain), attr)
        if response.status_code == 422:
            logging.warning(self.arvanmsg(response.json()[
                            "message"]+" {} {} {}".format(attr["type"], attr["name"], attr["value"])))
        elif response.status_code == 201:
            logging.info(self.arvanmsg(response.json()[
                         "message"]+" {} {} {}".format(attr["type"], attr["name"], attr["value"])))

    def domain_register(self,zones={}):
        print(''.join(
            [color.BOLD, "[*] select one of your zones to migrate :", color.END, "\n"]))
        number = 0
        domains = zones.keys()
        for domain in domains:
            print("{} [{}] {} {}\n".format(
                color.GREEN, number, domain, color.END))
            number = number + 1
        if number > 1:
            print("{} [{}] {} {}\n".format(
                color.GREEN, number, "all", color.END))
        target = input(color.UNDERLINE +
                       "Choose your domain number : "+color.END)
        if target == number+1:
            print("all")
        else:
            logging.debug(self.arvanmsg("Registering a domain"))
            payload = {"domain": domains[target]}
            response = self.post("domains/dns-service", payload)
            if response == 302:
                logging.warning(
                    self.arvanmsg("It seems that the domain has already been registered in Arvan !!"))
            else:
                # inja bayad sharte response salem check beshe
                logging.info(self.arvanmsg(response.json()["message"]))
            self.get_dns_recorde(zones[domains[target]], domains[target])
            firewall = Firewall(self.arvancloud_base_url+"domains/{}/firewall/rules".format(domains[target]),self.arvancloud_request_headers,self.cfkey)
            firewall.get_firewall_rules(zones[domains[target]])

