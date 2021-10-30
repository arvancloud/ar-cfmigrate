using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CfMigrate.Cloudflare.Models.Zone
{
    public class ZoneOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public bool Paused { get; set; }
        public string Type { get; set; }

        [JsonPropertyName("development_mode")]
        public int DevelopmentMode { get; set; }

        [JsonPropertyName("name_servers")]
        public List<string> NameServers { get; set; }

        [JsonPropertyName("original_name_servers")]
        public List<string> OriginalNameServers { get; set; }

        [JsonPropertyName("original_registrar")]
        public string OriginalRegistrar { get; set; }

        [JsonPropertyName("original_dnshost")]
        public object OriginalDnsHost { get; set; }

        [JsonPropertyName("modified_on")]
        public DateTime ModifiedOn { get; set; }

        [JsonPropertyName("created_on")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("activated_on")]
        public object ActivatedOn { get; set; }

        public Meta Meta { get; set; }
        public Owner Owner { get; set; }
        public Account Account { get; set; }
        public List<string> Permissions { get; set; }
        public Plan Plan { get; set; }
    }

    public class Meta
    {
        public int Step { get; set; }

        [JsonPropertyName("wildcard_proxiable")]
        public bool WildcardProxiable { get; set; }

        [JsonPropertyName("custom_certificate_quota")]
        public int CustomCertificateQuota { get; set; }

        [JsonPropertyName("page_rule_quota")]
        public int PageRuleQuota { get; set; }

        [JsonPropertyName("phishing_detected")]
        public bool PhishingDetected { get; set; }

        [JsonPropertyName("multiple_railguns_allowed")]
        public bool MultipleRailgunsAllowed { get; set; }
    }

    public class Owner
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
    }

    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Plan
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public string Frequency { get; set; }

        [JsonPropertyName("is_subscribed")]
        public bool IsSubscribed { get; set; }

        [JsonPropertyName("can_subscribe")]
        public bool CanSubscribe { get; set; }

        [JsonPropertyName("legacy_id")]
        public string LegacyId { get; set; }

        [JsonPropertyName("legacy_discount")]
        public bool LegacyDiscount { get; set; }

        [JsonPropertyName("externally_managed")]
        public bool ExternallyManaged { get; set; }
    }
}