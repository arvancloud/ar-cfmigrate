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

  async getDomains() {
    const req = await this.client.get('domains')
    return req.data.data.map(domain => domain.name)
  }

  async createDomain(domain) {
    const domains = await this.getDomains()
    if (!domains.includes(domain)) {
      await this.client.post(`/domains/dns-service`, {domain})
    }
  }

  _getClosestValidTTL(ttl) {
    const validTTLs = [120, 180, 300, 600, 900, 1800, 3600, 7200, 18000, 43200, 86400, 172800, 432000]
    return validTTLs.reduce(function(prev, curr) {
      return (Math.abs(curr - ttl) < Math.abs(prev - ttl) ? curr : prev);
    });
  }

  _getRecordValue(type, value, priority) {
    switch (type.toLowerCase()) {
      case 'a':
      case 'aaaa':
        return [{ip: value}]
      case 'cname':
        return {host: value, host_header: 'source'}
      case 'aname':
        return {location: value, host_header: 'source'}
      case 'ns':
        return {host: value}
      case 'mx':
        return {host: value, priority}
      case 'txt':
        return {text: value}
      case 'ptr':
        return {domain: value}
    }
  }

  async createDNSRecord(domain, type, name, value, ttl, cloud, priority=null) {
    ttl = this._getClosestValidTTL(ttl)
    cloud = cloud ? 'true' : 'false'
    value = this._getRecordValue(type, value, priority)
    type = type.toLowerCase()

    await this.client.post(`/domains/${domain}/dns-records`, {type, name, value, ttl, cloud})
  }
}

module.exports = ArvanClient
