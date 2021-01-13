const axios = require('axios')

class ArvanClient {
  constructor(apiKey) {
    this.client = axios.create({
      baseURL: 'https://napi.arvancloud.com/cdn/4.0/',
      timeout: 10000,
      headers: {
        authorization: apiKey,
      }
    });
  }

  async createDomain(domain) {
    await this.client.post(`/domains/dns-service`, {domain})
  }

  async createDNSRecord(domain, type, name, value, ttl, cloud) {
    this.client.post(`/domains/${domain}/dns-records`, {type, name, value, ttl, cloud})
  }
}

module.exports = ArvanClient
