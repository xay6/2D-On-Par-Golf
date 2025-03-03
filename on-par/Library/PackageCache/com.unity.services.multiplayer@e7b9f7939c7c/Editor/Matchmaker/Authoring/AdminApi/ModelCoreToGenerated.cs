using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Unity.Services.Multiplayer.Editor.Matchmaker.Authoring.AdminApi.Shared;
using Generated = Unity.Services.Multiplayer.Editor.Matchmaker.Authoring.AdminApi.Matchmaker.Model;
using CoreModel = Unity.Services.Multiplayer.Editor.Matchmaker.Authoring.Core.Model;
using AnyOf = Unity.Services.Multiplayer.Editor.Matchmaker.Authoring.Core.Model.JsonObject;

namespace Unity.Services.Multiplayer.Editor.Matchmaker.Authoring.AdminApi
{
    static class ModelCoreToGenerated
    {
        internal static (Generated.QueueConfig, List<CoreModel.ErrorResponse>) FromCoreQueueConfig(
            CoreModel.QueueConfig queueConfig,
            CoreModel.MultiplayResources availableMultiplayResources,
            bool dryRun)
        {
            var (defaultPool, errorResponses) = FromCoreBasePoolConfig(
                queueConfig.DefaultPool,
                availableMultiplayResources,
                dryRun);

            var filteredPools = queueConfig.FilteredPools?.Select(f => FromCoreFilteredPoolConfig(f, availableMultiplayResources, dryRun)).ToList();
            errorResponses.AddRange(filteredPools?.Select(f => f.Item2).SelectMany(f => f) ?? new List<CoreModel.ErrorResponse>());

            return (
                new Generated.QueueConfig(
                    name: queueConfig.Name.ToString() ?? string.Empty,
                    enabled: queueConfig.Enabled,
                    maxPlayersPerTicket: queueConfig.MaxPlayersPerTicket,
                    defaultPool: defaultPool,
                    filteredPools: filteredPools?.Select(f => f.Item1).ToList() ?? new List<Generated.FilteredPoolConfig>()
                ), errorResponses);
        }

        static (Generated.MatchHosting, List<CoreModel.ErrorResponse>) FromCoreMatchHosting(
            CoreModel.IMatchHostingConfig matchHostingConfig,
            CoreModel.MultiplayResources availableMultiplayResources,
            bool dryRun)
        {
            var errors = new List<CoreModel.ErrorResponse>();
            if (matchHostingConfig is CoreModel.MultiplayConfig multiplayConfig)
            {
                var fleet = availableMultiplayResources.Fleets.Find(f => f.Name == multiplayConfig.FleetName);
                if (fleet.Name == null)
                {
                    errors.Add(
                        new CoreModel.ErrorResponse()
                        {
                            ResultCode = "InvalidMultiplayFleetName",
                            Message = $"Fleet named '{multiplayConfig.FleetName}' not found."
                        });
                }
                else
                {
                    var qosRegion = fleet.QosRegions.Find(r => r.Name == multiplayConfig.DefaultQoSRegionName);
                    var buildConfig = fleet.BuildConfigs.Find(b => b.Name == multiplayConfig.BuildConfigurationName);
                    if (buildConfig.Name == null)
                    {
                        errors.Add(
                            new CoreModel.ErrorResponse()
                            {
                                ResultCode = "InvalidBuildConfigurationName",
                                Message = $"Build configuration named '{multiplayConfig.BuildConfigurationName}' not found in fleet named '{multiplayConfig.FleetName}'."
                            });
                    }
                    else if (qosRegion.Name == null)
                    {
                        errors.Add(
                            new CoreModel.ErrorResponse()
                            {
                                ResultCode = "InvalidDefaultQoSRegion",
                                Message = $"QoS region named '{multiplayConfig.DefaultQoSRegionName}' not found for fleet named '{multiplayConfig.FleetName}'."
                            });
                    }
                    else
                    {
                        if (!dryRun)
                        {
                            return (new Generated.MatchHosting(
                                new Generated.MultiplayHostingConfig(
                                    type: Generated.MultiplayHostingConfig.TypeEnum.Multiplay,
                                    fleetId: fleet.Id,
                                    buildConfigurationId: buildConfig.Id,
                                    defaultQoSRegionId: qosRegion.Id
                                )), errors);
                        }
                    }
                }
            }
            return (new Generated.MatchHosting(new Generated.MatchIdHostingConfig(type: Generated.MatchIdHostingConfig.TypeEnum.MatchId)), errors);
        }

        static Generated.Rule FromCoreRule(CoreModel.Rule rule)
        {
            Generated.Rule.TypeEnum type = rule.type switch
            {
                CoreModel.RuleType.GreaterThan => Generated.Rule.TypeEnum.GreaterThan,
                CoreModel.RuleType.GreaterThanEqual => Generated.Rule.TypeEnum.GreaterThanEqual,
                CoreModel.RuleType.LessThan => Generated.Rule.TypeEnum.LessThan,
                CoreModel.RuleType.LessThanEqual => Generated.Rule.TypeEnum.LessThanEqual,
                CoreModel.RuleType.Difference => Generated.Rule.TypeEnum.Difference,
                CoreModel.RuleType.DoubleDifference => Generated.Rule.TypeEnum.DoubleDifference,
                CoreModel.RuleType.Equality => Generated.Rule.TypeEnum.Equality,
                CoreModel.RuleType.InList => Generated.Rule.TypeEnum.InList,
                CoreModel.RuleType.Intersection => Generated.Rule.TypeEnum.Intersection,
                _ => throw new InvalidEnumArgumentException(nameof(type))
            };

            Generated.RuleExternalDataCloudSave.AccessClassEnum accessClass =
                rule.externalData?.cloudSave?.accessClass switch
                {
                    CoreModel.RuleExternalData.CloudSave.AccessClass.Public => Generated.RuleExternalDataCloudSave
                        .AccessClassEnum.Public,
                    CoreModel.RuleExternalData.CloudSave.AccessClass.Private => Generated.RuleExternalDataCloudSave
                        .AccessClassEnum.Private,
                    CoreModel.RuleExternalData.CloudSave.AccessClass.Protected => Generated.RuleExternalDataCloudSave
                        .AccessClassEnum.Protected,
                    _ => Generated.RuleExternalDataCloudSave.AccessClassEnum.Default,
                };

            var generatedRule = new Generated.Rule(
                source: rule.source,
                name: rule.name,
                type: type,
                enableRule: rule.enableRule,
                not: rule.not,
                reference: new ApiObject(rule.reference),
                overlap: Convert.ToDecimal(rule.overlap, CultureInfo.InvariantCulture),
                relaxations: rule.relaxations?.Select(FromCoreRuleRelaxation).ToList() ??
                             new List<Generated.RuleRelaxation>()
            );

            if (rule.externalData != null)
            {
                generatedRule.ExternalData = new Generated.RuleExternalData();
                if (rule.externalData.leaderboard != null)
                {
                    generatedRule.ExternalData.Leaderboard =
                        new Generated.RuleExternalDataLeaderboard(rule.externalData.leaderboard.id);
                }

                if (rule.externalData.cloudSave != null)
                {
                    generatedRule.ExternalData.CloudSave = new Generated.RuleExternalDataCloudSave(
                        _default: new ApiObject(rule.externalData.cloudSave._default),
                        accessClass: accessClass);
                }
            }

            return generatedRule;
        }

        static Generated.AgeType FromCoreAgeType(CoreModel.AgeType ageType)
        {
            return ageType switch
            {
                CoreModel.AgeType.Average => Generated.AgeType.Average,
                CoreModel.AgeType.Oldest => Generated.AgeType.Oldest,
                CoreModel.AgeType.Youngest => Generated.AgeType.Youngest,
                _ => throw new InvalidEnumArgumentException(nameof(ageType))
            };
        }

        static Generated.RuleRelaxation FromCoreRuleRelaxation(CoreModel.RuleRelaxation ruleRelaxation)
        {
            Generated.RuleRelaxation.TypeEnum type = ruleRelaxation.type switch
            {
                CoreModel.RuleRelaxationType.ReferenceControlReplace =>
                    Generated.RuleRelaxation.TypeEnum.ReferenceControlReplace,
                CoreModel.RuleRelaxationType.RuleControlDisable => Generated.RuleRelaxation.TypeEnum.RuleControlDisable,
                CoreModel.RuleRelaxationType.RuleControlEnable => Generated.RuleRelaxation.TypeEnum.RuleControlEnable,
                _ => throw new InvalidEnumArgumentException(nameof(type))
            };

            return new Generated.RuleRelaxation(
                type: type,
                atSeconds: ruleRelaxation.atSeconds,
                ageType: FromCoreAgeType(ruleRelaxation.ageType),
                value: new ApiObject(ruleRelaxation.value)
            );
        }

        static Generated.RangeRelaxation FromCoreRangeRelaxation(CoreModel.RangeRelaxation rangeRelaxation)
        {
            return new Generated.RangeRelaxation(
                type: Generated.RangeRelaxation.TypeEnum.RangeControlReplaceMin,
                atSeconds: rangeRelaxation.atSeconds,
                ageType: FromCoreAgeType(rangeRelaxation.ageType),
                value: rangeRelaxation.value
            );
        }

        static Generated.RuleBasedMatchDefinition FromCoreRuleBasedMatchDefinition(
            CoreModel.RuleBasedMatchDefinition ruleBasedMatchDefinition)
        {
            return new Generated.RuleBasedMatchDefinition(
                teams: ruleBasedMatchDefinition.teams?.Select(
                        team => new Generated.RuleBasedTeamDefinition(
                            name: team.name,
                            teamCount: new Generated.Range(
                                min: team.teamCount.min,
                                max: team.teamCount.max,
                                relaxations: team.teamCount.relaxations?.Select(FromCoreRangeRelaxation).ToList() ??
                                             new List<Generated.RangeRelaxation>()
                            ),
                            playerCount: new Generated.Range(
                                min: team.playerCount.min,
                                max: team.playerCount.max,
                                relaxations: team.playerCount.relaxations?.Select(FromCoreRangeRelaxation).ToList() ??
                                             new List<Generated.RangeRelaxation>()
                            ),
                            teamRules: team.teamRules?.Select(FromCoreRule).ToList() ?? new List<Generated.Rule>())
                    )
                    .ToList() ?? new List<Generated.RuleBasedTeamDefinition>(),
                matchRules: ruleBasedMatchDefinition.matchRules?.Select(FromCoreRule).ToList() ??
                            new List<Generated.Rule>()
            );
        }

        static Generated.Rules FromCoreMatchLogic(CoreModel.MatchLogicRulesConfig matchRules)
        {
            return new Generated.Rules(
                name: matchRules.Name,
                backfillEnabled: matchRules.BackfillEnabled,
                matchDefinition: FromCoreRuleBasedMatchDefinition(matchRules.MatchDefinition)
            );
        }

        static (Generated.PoolConfig, List<CoreModel.ErrorResponse>) FromCorePoolConfig(
            CoreModel.PoolConfig poolConfig,
            CoreModel.MultiplayResources availableMultiplayResources,
            bool dryRun)
        {
            var matchHosting = FromCoreMatchHosting(poolConfig.MatchHosting, availableMultiplayResources, dryRun);
            return (new Generated.PoolConfig(
                name: poolConfig.Name.ToString() ?? string.Empty,
                enabled: poolConfig.Enabled,
                timeoutSeconds: poolConfig.TimeoutSeconds,
                matchLogic: FromCoreMatchLogic(poolConfig.MatchLogic),
                matchHosting: matchHosting.Item1
            ), matchHosting.Item2);
        }

        static (Generated.FilteredPoolConfig, List<CoreModel.ErrorResponse>) FromCoreFilteredPoolConfig(
            CoreModel.FilteredPoolConfig poolConfig,
            CoreModel.MultiplayResources availableMultiplayResources,
            bool dryRun)
        {
            var (matchHosting, errors) = FromCoreMatchHosting(
                poolConfig.MatchHosting,
                availableMultiplayResources,
                dryRun);

            var variants = poolConfig.Variants?.Select(p => FromCorePoolConfig(p, availableMultiplayResources, dryRun)).ToList();
            errors.AddRange(variants?.Select(v => v.Item2).SelectMany(e => e).ToList() ?? new List<CoreModel.ErrorResponse>());

            return (new Generated.FilteredPoolConfig(
                name: poolConfig.Name.ToString() ?? string.Empty,
                enabled: poolConfig.Enabled,
                timeoutSeconds: poolConfig.TimeoutSeconds,
                matchLogic: FromCoreMatchLogic(poolConfig.MatchLogic),
                matchHosting: matchHosting,
                variants: variants?.Select(v => v.Item1).ToList(),
                filters: poolConfig.Filters?.Select(
                        f =>
                        {
                            Generated.Filter.OperatorEnum filter = f.Operator switch
                            {
                                CoreModel.FilteredPoolConfig.Filter.FilterOperator.GreaterThan => Generated.Filter
                                    .OperatorEnum
                                    .GreaterThan,
                                CoreModel.FilteredPoolConfig.Filter.FilterOperator.LessThan => Generated.Filter.OperatorEnum
                                    .LessThan,
                                CoreModel.FilteredPoolConfig.Filter.FilterOperator.NotEqual => Generated.Filter.OperatorEnum
                                    .NotEqual,
                                CoreModel.FilteredPoolConfig.Filter.FilterOperator.Equal => Generated.Filter.OperatorEnum
                                    .Equal,
                                _ => throw new InvalidEnumArgumentException(nameof(f.Operator))
                            };

                            return new Generated.Filter(
                                attribute: f.Attribute,
                                _operator: filter,
                                value: new ApiObject(f.Value)
                            );
                        })
                    .ToList() ?? new List<Generated.Filter>()
            ), errors);
        }

        static (Generated.BasePoolConfig, List<CoreModel.ErrorResponse>) FromCoreBasePoolConfig(
            CoreModel.BasePoolConfig poolConfig,
            CoreModel.MultiplayResources availableMultiplayResources,
            bool dryRun)
        {
            if (poolConfig == null)
            {
                return (null, new List<CoreModel.ErrorResponse>());
            }

            var (matchHosting, errors) = FromCoreMatchHosting(
                poolConfig.MatchHosting,
                availableMultiplayResources,
                dryRun);

            var variants = poolConfig.Variants?.Select(p => FromCorePoolConfig(p, availableMultiplayResources, dryRun)).ToList();
            errors.AddRange(variants?.Select(v => v.Item2).SelectMany(e => e).ToList() ?? new List<CoreModel.ErrorResponse>());

            return (new Generated.BasePoolConfig(
                name: poolConfig.Name.ToString() ?? string.Empty,
                enabled: poolConfig.Enabled,
                timeoutSeconds: poolConfig.TimeoutSeconds,
                matchLogic: FromCoreMatchLogic(poolConfig.MatchLogic),
                matchHosting: matchHosting,
                variants: variants?.Select(v => v.Item1).ToList() ?? new List<Generated.PoolConfig>()
            ), errors);
        }
    }
}
