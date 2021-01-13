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

  _getClosestValidTTL(ttl) {
    const validTTLs = [120, 180, 300, 600, 900, 1800, 3600, 7200, 18000, 43200, 86400, 172800, 432000]
    return validTTLs.reduce(function(prev, curr) {
      return (Math.abs(curr - ttl) < Math.abs(prev - ttl) ? curr : prev);
    });
  }

  async createDNSRecord(domain, type, name, value, ttl, cloud) {
    ttl = this._getClosestValidTTL(ttl)
    cloud = cloud ? 'true' : 'false'
    value = [{ip: value}]
    await this.client.post(`/domains/${domain}/dns-records`, {type, name, value, ttl, cloud})
  }
}

module.exports = ArvanClient
