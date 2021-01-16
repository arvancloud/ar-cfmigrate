# ar-cfmigrate
CloudFlare to ArvanCloud CDN Migration Tool

# Usage
```
# NPM
npm i -g cf2ar
cf2ar 

# DOCKER 
docker run -it sadeghhayeri/cf2ar:1.1.0
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

---
# Brief
A tool to migrate a Domain(DNS-CDN-...) configs from Cloudflare panel to Arvancloud (as much as possible)

## Input
User Arvancloud API Token 
User Cloudflare API Token

## Capabalities
* Register Domain in ArvanCloud DNS
* Export DNS records from Cloudflare and Import to Arvancloud Panel
* Config Cache TTL and settings
* Config firewall rules 
* ...

## Useful Links
[Arvancloud CDN API Documentation](https://www.arvancloud.com/docs/api/cdn/4.0)
[Cloudflare CDN API Documentation](https://api.cloudflare.com/)


## Terms and Conditions
* All projects received to ArvanCloud will be reviewed, and the price will be paid to the first approved project.
* All projects have to have test and execution document.
* The project doer has to solve issues and apply required changes for 6 months after approval of the project.
* General changes or changing programming language in each project has to be approved by ArvanCloud.
* In case more than one project is approved by ArvanCLoud, the project fee will be equally divided between winning projects.
* In the evaluation and code reviews stages of a project, no new request for the same project will be accepted. In case the reviewed project fails to pass the quality assessments, the project will be reactivated.
* If projects require any update or edit, merge requests will be accepted in GitHub after reassessment and reapproval.
* Approved projects will be forked in GitHub, and ArvanCloud will star them.
* GitHub name and address of the approved project doer will be published alongside the project. 
