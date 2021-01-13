const prompts = require('prompts')
const cloudflare = require('cloudflare')
const ora = require('ora')
const ArvanCloud = require('./arvanclient')


async function cli() {
  const arvanKey = await prompts([
    {
      type: 'text',
      name: 'apikey',
      message: 'enter your arvan API key',
    }
  ])
  const arvan = new ArvanCloud(arvanKey.apikey)

  const cloudflareKeyType = await prompts([
    {
      type: 'text',
      name: 'cloudflareKeyType',
      message: 'select cloudflare key type [api-key or api-token]',
      validate: value =>  ['api-key', 'api-token'].includes(value),
    }
  ])

  let cf
  if (cloudflareKeyType.cloudflareKeyType === 'api-key') {
    const values = await prompts([
      {
        type: 'text',
        name: 'email',
        message: 'enter your email',
      },
      {
        type: 'text',
        name: 'key',
        message: 'enter your key',
      }
    ])
    cf = cloudflare(values)
  } else {
    const values = await prompts([
      {
        type: 'text',
        name: 'token',
        message: 'enter your token',
      }
    ])
    cf = cloudflare(values)
  }

  let spinner = ora(`Loading zones`).start();
  const zones = (await cf.zones.browse()).result
  spinner.succeed()

  const selectedZone = await prompts([
    {
      type: 'select',
      name: 'zone',
      message: 'Select target zone',
      choices: zones.map(zone => ({title: zone.name, value: zone})),
    }
  ])

  spinner = ora(`Create domain ${selectedZone.zone.name} in arvan`).start();
  try {
    await arvan.createDomain(selectedZone.zone.name)
  } catch(e) {
    return spinner.fail('Error on creating domain in Arvan')
  }
  spinner.succeed()

  spinner = ora(`Loading cloudflare DNS records`).start();
  const dnsRecords = (await cf.dnsRecords.browse(selectedZone.zone.id)).result
  spinner.succeed()

  for(let dnsRecord of dnsRecords) {
    spinner = ora(`Add DNS record ${dnsRecord.name}`).start();
    try {
      await arvan.createDNSRecord(
        selectedZone.zone.name,
        dnsRecord.type,
        dnsRecord.name.substr(0, dnsRecord.name.length - selectedZone.zone.name.length - 1),
        dnsRecord.content,
        dnsRecord.ttl,
        dnsRecord.proxied,
      )
    } catch(e) {
      spinner.fail(`Error on creating DNS Record ${dnsRecord.name}`)
    }
    spinner.succeed()
  }
}

cli()