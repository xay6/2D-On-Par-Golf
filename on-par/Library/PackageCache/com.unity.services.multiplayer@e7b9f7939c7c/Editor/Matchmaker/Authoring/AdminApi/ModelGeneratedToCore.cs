using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using Generated = Unity.Services.Multiplayer.Editor.Matchmaker.Authoring.AdminApi.Matchmaker.Model;
using CoreModel = Unity.Services.Multiplayer.Editor.Matchmaker.Authoring.Core.Model;

namespace Unity.Services.Multiplayer.Editor.Matchmaker.Authoring.AdminApi
{
    static class ModelGeneratedToCore
    {
        static CoreModel.Rule FromGeneratedRule(Generated.Rule rule)
        {
            CoreModel.RuleType type = rule.Type switch
            {
                Generated.Rule.TypeEnum.GreaterThan => CoreModel.RuleType.GreaterThan,
                Generated.Rule.TypeEnum.GreaterThanEqual => CoreModel.RuleType.GreaterThanEqual,
                Generated.Rule.TypeEnum.LessThan => CoreModel.RuleType.LessThan,
                Generated.Rule.TypeEnum.LessThanEqual => CoreModel.RuleType.LessThanEqual,
                Generated.Rule.TypeEnum.Difference => CoreModel.RuleType.Difference,
                Generated.Rule.TypeEnum.DoubleDifference => CoreModel.RuleType.DoubleDifference,
                Generated.Rule.TypeEnum.Equality => CoreModel.RuleType.Equality,
                Generated.Rule.TypeEnum.InList => CoreModel.RuleType.InList,
                Generated.Rule.TypeEnum.Intersection => CoreModel.RuleType.Intersection,
                _ => throw new InvalidEnumArgumentException(nameof(type))
            };

            CoreModel.RuleExternalData.CloudSave.AccessClass accessClass = rule.ExternalData?.CloudSave?.AccessClass switch
            {
                Generated.RuleExternalDataCloudSave.AccessClassEnum.Public => CoreModel.RuleExternalData.CloudSave.AccessClass.Public,
                Generated.RuleExternalDataCloudSave.AccessClassEnum.Private => CoreModel.RuleExternalData.CloudSave.AccessClass.Private,
                Generated.RuleExternalDataCloudSave.AccessClassEnum.Protected => CoreModel.RuleExternalData.CloudSave.AccessClass.Protected,
                _ => CoreModel.RuleExternalData.CloudSave.AccessClass.Default,
            };

            CoreModel.RuleExternalData externalData = null;
            if (rule.ExternalData != null)
            {
                externalData = new CoreModel.RuleExternalData();

                if (rule.ExternalData.Leaderboard != null)
                {
                    externalData.leaderboard = new CoreModel.RuleExternalData.Leaderboard
                    {
                        id = rule.ExternalData.Leaderboard.Id
                    };
                }

                if (rule.ExternalData.CloudSave != null)
                {
                    externalData.cloudSave = new CoreModel.RuleExternalData.CloudSave()
                    {
                        accessClass = accessClass,
                        _default = new MatchmakerAdminClient.JsonObjectSpecialized(
                            JsonConvert.SerializeObject(rule.ExternalData.CloudSave.Default))
                    };
                }
            }
            return new CoreModel.Rule
            {
                source = rule.Source,
                name = rule.Name,
                type = type,
                enableRule = rule.EnableRule,
                not = rule.Not,
                reference = new MatchmakerAdminClient.JsonObjectSpecialized(JsonConvert.SerializeObject(rule.Reference)),
                overlap = Decimal.ToDouble(rule.Overlap),
                relaxations = rule.Relaxations?.Select(
                        rlx =>
                        {
                            CoreModel.RuleRelaxationType rlxType = rlx.Type switch
                            {
                                Generated.RuleRelaxation.TypeEnum.ReferenceControlReplace => CoreModel.RuleRelaxationType.ReferenceControlReplace,
                                Generated.RuleRelaxation.TypeEnum.RuleControlDisable => CoreModel.RuleRelaxationType.RuleControlDisable,
                                Generated.RuleRelaxation.TypeEnum.RuleControlEnable => CoreModel.RuleRelaxationType.RuleControlEnable,
                                _ => throw new InvalidEnumArgumentException(nameof(rlxType))
                            };

                            var coreRuleRelaxation = new CoreModel.RuleRelaxation
                            {
                                type = rlxType,
                                atSeconds = rlx.AtSeconds,
                                ageType = FromGeneratedAgeType(rlx.AgeType)
                            };

                            if (rlx.Value != null)
                            {
                                coreRuleRelaxation.value = new MatchmakerAdminClient.JsonObjectSpecialized(JsonConvert.SerializeObject(rlx.Value));
                            }

                            return coreRuleRelaxation;
                        })
                    .ToList() ?? new List<CoreModel.RuleRelaxation>(),
                externalData = externalData
            };
        }

        static CoreModel.RangeRelaxation FromGeneratedRangeRelaxation(Generated.RangeRelaxation rangeRelaxation)
        {
            return new CoreModel.RangeRelaxation()
            {
                type = CoreModel.RangeRelaxationType.RangeControlReplaceMin,
                atSeconds = rangeRelaxation.AtSeconds,
                ageType = FromGeneratedAgeType(rangeRelaxation.AgeType),
                value = rangeRelaxation.Value
            };
        }

        static CoreModel.AgeType FromGeneratedAgeType(Generated.AgeType ageType)
        {
            return ageType switch
            {
                Generated.AgeType.Average => CoreModel.AgeType.Average,
                Generated.AgeType.Oldest => CoreModel.AgeType.Oldest,
                Generated.AgeType.Youngest => CoreModel.AgeType.Youngest,
                _ => throw new InvalidEnumArgumentException(nameof(ageType))
            };
        }

        static CoreModel.RuleBasedMatchDefinition FromGeneratedRuleBase(Generated.RuleBasedMatchDefinition matchDefinition)
        {
            return new CoreModel.RuleBasedMatchDefinition
            {
                matchRules = matchDefinition.MatchRules?.Select(FromGeneratedRule).ToList() ?? new List<CoreModel.Rule>(),
                teams = matchDefinition.Teams?.Select(
                        team => new CoreModel.RuleBasedTeamDefinition
                        {
                            name = team.Name,
                            playerCount = new CoreModel.Range
                            {
                                min = team.PlayerCount.Min,
                                max = team.PlayerCount.Max,
                                relaxations = team.PlayerCount.Relaxations?.Select(FromGeneratedRangeRelaxation).ToList() ?? new List<CoreModel.RangeRelaxation>()
                            },
                            teamCount = new CoreModel.Range
                            {
                                min = team.TeamCount.Min,
                                max = team.TeamCount.Max,
                                relaxations = team.TeamCount.Relaxations?.Select(FromGeneratedRangeRelaxation).ToList() ?? new List<CoreModel.RangeRelaxation>()
                            },
                            teamRules = team.TeamRules?.Select(FromGeneratedRule).ToList() ?? new List<CoreModel.Rule>()
                        })
                    .ToList() ?? new List<CoreModel.RuleBasedTeamDefinition>()
            };
        }

        static CoreModel.MatchLogicRulesConfig FromGeneratedMatchLogic(Generated.Rules matchLogic)
        {
            return new CoreModel.MatchLogicRulesConfig
            {
                Name = matchLogic.Name,
                BackfillEnabled = matchLogic.BackfillEnabled,
                MatchDefinition = FromGeneratedRuleBase(matchLogic.MatchDefinition)
            };
        }

        static (CoreModel.IMatchHostingConfig, List<CoreModel.ErrorResponse>) FromGeneratedHostingConfig(Generated.MatchHosting matchHosting, CoreModel.MultiplayResources availableMultiplayResources)
        {
            CoreModel.IMatchHostingConfig matchHostingConfig;
            var errors = new List<CoreModel.ErrorResponse>();
            var buildName = string.Empty;
            var regionName = string.Empty;
            if (matchHosting.ActualInstance is Generated.MultiplayHostingConfig multiplayConfig)
            {
                var fleet = availableMultiplayResources.Fleets.Find(f => f.Id == multiplayConfig.FleetId);
                if (fleet.Name == null)
                {
                    errors.Add(
                        new CoreModel.ErrorResponse()
                        {
                            ResultCode = "InvalidMultiplayFleetId",
                            Message = $"Fleet with id '{multiplayConfig.FleetId}' not found."
                        });
                }
                else
                {
                    regionName = fleet.QosRegions.Find(r => r.Id == multiplayConfig.DefaultQoSRegionId).Name;
                    buildName = fleet.BuildConfigs.Find(b => b.Id == multiplayConfig.BuildConfigurationId).Name;
                    if (buildName == null)
                    {
                        errors.Add(
                            new CoreModel.ErrorResponse()
                            {
                                ResultCode = "InvalidBuildConfigurationId",
                                Message = $"Build configuration with id '{multiplayConfig.BuildConfigurationId}' not found in fleet named '{fleet.Name}'."
                            });
                    }
                    else if (regionName == null)
                    {
                        errors.Add(
                            new CoreModel.ErrorResponse()
                            {
                                ResultCode = "InvalidDefaultQoSRegion",
                                Message = $"QoS region named '{multiplayConfig.DefaultQoSRegionId}' not found for fleet named '{fleet.Name}'."
                            });
                    }
                }
                matchHostingConfig = new CoreModel.MultiplayConfig
                {
                    Type = CoreModel.IMatchHostingConfig.MatchHostingType.Multiplay,
                    FleetName = fleet.Name,
                    BuildConfigurationName = buildName,
                    DefaultQoSRegionName = regionName
                };
            }
            else
            {
                matchHostingConfig = new CoreModel.MatchIdConfig()
                {
                    Type = CoreModel.IMatchHostingConfig.MatchHostingType.MatchId
                };
            }

            return (matchHostingConfig, errors);
        }

        internal static (CoreModel.QueueConfig, List<CoreModel.ErrorResponse>) FromGeneratedQueueConfig(Generated.QueueConfig queueConfig, CoreModel.MultiplayResources availableMultiplayResources)
        {
            var (defaultPool, errors) = FromGeneratedBasePoolConfig(queueConfig.DefaultPool, availableMultiplayResources);
            var filteredPools = queueConfig.FilteredPools?.Select(v => FromGeneratedFilteredPoolConfig(v, availableMultiplayResources)).ToList();
            errors.AddRange(filteredPools?.Select(v => v.Item2).SelectMany(e => e) ?? new List<CoreModel.ErrorResponse>());
            return (new CoreModel.QueueConfig
            {
                Name = new CoreModel.QueueName(queueConfig.Name),
                Enabled = queueConfig.Enabled,
                MaxPlayersPerTicket = queueConfig.MaxPlayersPerTicket,
                DefaultPool = defaultPool,
                FilteredPools = filteredPools?.Select(v => v.Item1).ToList() ?? new List<CoreModel.FilteredPoolConfig>(),
            }, errors);
        }

        static (CoreModel.BasePoolConfig, List<CoreModel.ErrorResponse>) FromGeneratedBasePoolConfig(Generated.BasePoolConfig poolConfig, CoreModel.MultiplayResources availableMultiplayResources)
        {
            if (poolConfig == null)
            {
                return (null, new List<CoreModel.ErrorResponse>());
            }
            var (matchHosting, errors) = FromGeneratedHostingConfig(poolConfig.MatchHosting, availableMultiplayResources);
            var variants = poolConfig.Variants?.Select(v => FromGeneratedPoolConfig(v, availableMultiplayResources)).ToList();
            errors.AddRange(variants?.Select(v => v.Item2).SelectMany(e => e) ?? new List<CoreModel.ErrorResponse>());
            return (new CoreModel.BasePoolConfig
            {
                Name = new CoreModel.PoolName(poolConfig.Name),
                Enabled = poolConfig.Enabled,
                MatchLogic = FromGeneratedMatchLogic(poolConfig.MatchLogic),
                MatchHosting = matchHosting,
                TimeoutSeconds = poolConfig.TimeoutSeconds,
                Variants = variants?.Select(v => v.Item1).ToList() ?? new List<CoreModel.PoolConfig>(),
            }, errors);
        }

        static (CoreModel.PoolConfig, List<CoreModel.ErrorResponse>) FromGeneratedPoolConfig(Generated.PoolConfig poolConfig, CoreModel.MultiplayResources availableMultiplayResources)
        {
            var (matchHosting, errors) = FromGeneratedHostingConfig(poolConfig.MatchHosting, availableMultiplayResources);
            return (new CoreModel.PoolConfig
            {
                Name = new CoreModel.PoolName(poolConfig.Name),
                Enabled = poolConfig.Enabled,
                MatchLogic = FromGeneratedMatchLogic(poolConfig.MatchLogic),
                MatchHosting = matchHosting,
                TimeoutSeconds = poolConfig.TimeoutSeconds
            }, errors);
        }

        static (CoreModel.FilteredPoolConfig, List<CoreModel.ErrorResponse>) FromGeneratedFilteredPoolConfig(Generated.FilteredPoolConfig poolConfig, CoreModel.MultiplayResources availableMultiplayResources)
        {
            var (matchHosting, errors) = FromGeneratedHostingConfig(poolConfig.MatchHosting, availableMultiplayResources);
            var variants = poolConfig.Variants?.Select(v => FromGeneratedPoolConfig(v, availableMultiplayResources)).ToList();
            errors.AddRange(variants?.Select(v => v.Item2).SelectMany(e => e) ?? new List<CoreModel.ErrorResponse>());
            return (new CoreModel.FilteredPoolConfig
            {
                Name = new CoreModel.PoolName(poolConfig.Name),
                Enabled = poolConfig.Enabled,
                MatchLogic = FromGeneratedMatchLogic(poolConfig.MatchLogic),
                MatchHosting = matchHosting,
                TimeoutSeconds = poolConfig.TimeoutSeconds,
                Variants = variants?.Select(v => v.Item1).ToList() ?? new List<CoreModel.PoolConfig>(),
                Filters = poolConfig.Filters?.Select(
                        f =>
                        {
                            CoreModel.FilteredPoolConfig.Filter.FilterOperator filter = f.Operator switch
                            {
                                Generated.Filter.OperatorEnum.GreaterThan => CoreModel.FilteredPoolConfig.Filter.FilterOperator.GreaterThan,
                                Generated.Filter.OperatorEnum.LessThan => CoreModel.FilteredPoolConfig.Filter.FilterOperator.LessThan,
                                Generated.Filter.OperatorEnum.NotEqual => CoreModel.FilteredPoolConfig.Filter.FilterOperator.NotEqual,
                                Generated.Filter.OperatorEnum.Equal => CoreModel.FilteredPoolConfig.Filter.FilterOperator.Equal,
                                _ => throw new InvalidEnumArgumentException(nameof(filter))
                            };

                            return new CoreModel.FilteredPoolConfig.Filter
                            {
                                Attribute = f.Attribute,
                                Operator = filter,
                                Value = new MatchmakerAdminClient.JsonObjectSpecialized(JsonConvert.SerializeObject(f.Value))
                            };
                        })
                    .ToList() ?? new List<CoreModel.FilteredPoolConfig.Filter>()
            }, errors);
        }
    }
}
