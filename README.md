# ar-cfmigrate
CloudFlare to ArvanCloud CDN Migration Tool

# Brief
A tool to migrate a Domain(DNS-CDN-...) configs from Cloudflare panel to Arvancloud (as much as possible)

## Input
User Arvancloud API Token 
User Cloudflare API Token

# Usage
```
# DOCKER 
git clone git@github.com:arvancloud/ar-cfmigrate.git && cd ar-cfmigrate 
docker build -t ar-cfmigrate .
docker run -it ar-cfmigrate

# NODE
git clone git@github.com:arvancloud/ar-cfmigrate.git && cd ar-cfmigrate/src && npm i
./cli.js
```

### Example
```
? enter your arvan API key › # Enter your Arvan api-key

? select cloudflare key type [api-key or api-token] › # Enter cloudflare key type

? enter your token › # Enter cloudflare token

# select one of your zones to migrate!
? Select target zone › - Use arrow-keys. Return to submit.
❯   example.com
    example.ir
  ↓ example.me

✔ Loading zones
✔ Select target zone › example.com
✔ Create domain example.com in arvan

✔ Loading cloudflare DNS records

✔ Add A Record api.example.com [1.1.1.1]
✔ Add A Record panel.example.com [2.2.2.2]
✔ Add A Record staging.example.com [3.3.3.3]
✔ Add A Record example.com [185.120.221.247]
✔ Add CNAME Record api-staging.example.com [staging.example.com]
✔ Add CNAME Record tracking.example.com [api.elasticemail.com]
✔ Add MX Record example.com [mx3.zoho.com]
✔ Add MX Record example.com [mx2.zoho.com]
✔ Add MX Record example.com [mx.zoho.com]
✔ Add TXT Record api._domainkey.example.com [k=rsa;t=s;p=MIGfM...jtwIDAQAB]
✔ Add TXT Record _dmarc.example.com [v=DMARC1;p=none]
✔ Add TXT Record emails._domainkey.example.com [v=DKIM1; k=rsa; p=MIGfMA0GC...QlZ3QIDAQAB]
✔ Add TXT Record example.com [zoho-verification=zb23382325.zmverify.zoho.com]
```

## Capabalities
- [x] Register Domain in ArvanCloud DNS
- [x] Export DNS records from Cloudflare and Import to Arvancloud Panel
- [ ] Config Cache TTL and settings
- [ ] Config firewall rules 

### 👨🏻‍💻 Contributors:
- SadeghHayeri [![https://github.com/sadeghhayeri](https://img.shields.io/github/followers/sadeghhayeri?color=red&label=Follow&logo=github&style=flat-square)](https://github.com/sadeghhayeri)
