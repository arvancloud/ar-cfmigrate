#! /usr/bin/python
from arvanclient import *
import argparse


example_text = '''example:
 python main.py -arkey 'Apikey xxxxx' -cfkey 'xxxxx'
'''   
parser = argparse.ArgumentParser(description='CloudFlare to ArvanCloud CDN Migration Tool.',epilog=example_text)
parser.add_argument('-arkey', metavar='--arvan-api-key', type=str, help='set arvancloud api key', required=True)
parser.add_argument('-cfkey', metavar='--cloudflare-api-key', type=str, help='set cloudflare api key', required=True)
parser.add_argument('-v', action='store_true', help='provides additional details')                 
args = parser.parse_args()

if __name__ == '__main__':
    if args.v :
        arvan = ArvanClient(args.arkey,args.cfkey,10)
    else:
        arvan = ArvanClient(args.arkey,args.cfkey,20)
    arvan.check_cloudflare_apikey()
    arvan.check_arvancloud_apikey()
    arvan.get_domains()
