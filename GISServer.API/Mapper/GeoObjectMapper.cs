using GISServer.API.Model;
using GISServer.Domain.Model;
using NetTopologySuite.Geometries;


namespace GISServer.API.Mapper
{
    public class GeoObjectMapper
    {
        public async Task<GeoObjectDTO> ObjectToDTO(GeoObject geoObject)
        {
            GeoObjectDTO geoObjectDTO = new GeoObjectDTO();
            await Task.Run(async () =>
            {
                geoObjectDTO.Id = geoObject.Id;
                geoObjectDTO.Name = geoObject.Name;
                geoObjectDTO.GeoNameId = geoObject.GeoNameId;
                geoObjectDTO.Status = geoObject.Status;
                geoObjectDTO.UpdateTime = geoObject.UpdateTime;
                geoObjectDTO.CreationTime = geoObject.CreationTime;
                if (geoObject.GeoNameFeatureId != null)
                {
                    geoObjectDTO.GeoNameFeature = new GeoNamesFeatureDTO
                    {

                        Id = geoObject.GeoNameFeature.Id,
                        GeoNamesFeatureCode = geoObject.GeoNameFeature.GeoNamesFeatureCode,
                        GeoNamesFeatureKind = geoObject.GeoNameFeature.GeoNamesFeatureKind,
                        FeatureKindNameEn = geoObject.GeoNameFeature.FeatureKindNameEn,
                        FeatureNameEn = geoObject.GeoNameFeature.FeatureNameEn,
                        FeatureKindNameRu = geoObject.GeoNameFeature.FeatureKindNameRu,
                        FeatureNameRu = geoObject.GeoNameFeature.FeatureNameRu,
                        CommentsEn = geoObject.GeoNameFeature.CommentsEn,
                        CommentsRu = geoObject.GeoNameFeature.CommentsRu,
                    };
                }
                if (geoObject.GeometryVersion != null)
                {
                    foreach (var geometryVersion in geoObject.GeometryVersion)
                    {
                        geoObjectDTO.GeometryVersion.Add(new GeometryVersionDTO
                        {
                            Id = geometryVersion.Id,
                            AuthoritativeKnowledgeSource = geometryVersion.AuthoritativeKnowledgeSource,
                            Version = geometryVersion.Version,
                            Status = geometryVersion.Status,
                            ArchiveTime = geometryVersion.ArchiveTime,
                            UpdateTime = geometryVersion.UpdateTime,
                            CreationTime = geometryVersion.CreationTime,
                            BorderGeocodes = geometryVersion.BorderGeocodes,
                            AreaValue = geometryVersion.AreaValue,
                            WestToEastLength = geometryVersion.WestToEastLength,
                            NorthToSouthLength = geometryVersion.NorthToSouthLength
                        });
                    }
                }
                if (geoObject.Geometry != null)
                {
                    geoObjectDTO.Geometry = new GeometryVersionDTO
                    {
                        Id = geoObject.Geometry.Id,
                        AuthoritativeKnowledgeSource = geoObject.Geometry.AuthoritativeKnowledgeSource,
                        Version = geoObject.Geometry.Version,
                        Status = geoObject.Geometry.Status,
                        ArchiveTime = geoObject.Geometry.ArchiveTime,
                        UpdateTime = geoObject.Geometry.UpdateTime,
                        CreationTime = geoObject.Geometry.CreationTime,
                        BorderGeocodes = geoObject.Geometry.BorderGeocodes,
                        AreaValue = geoObject.Geometry.AreaValue,
                        WestToEastLength = geoObject.Geometry.WestToEastLength,
                        NorthToSouthLength = geoObject.Geometry.NorthToSouthLength
                    };
                }
                if (geoObject.GeoObjectInfo != null)
                {
                    geoObjectDTO.GeoObjectInfo = new GeoObjectInfoDTO
                    {
                        Id = geoObject.GeoObjectInfo.Id,
                        FullName = geoObject.GeoObjectInfo.FullName,
                        ShortName = geoObject.GeoObjectInfo.ShortName,
                        AuthoritativeKnowledgeSource = geoObject.GeoObjectInfo.AuthoritativeKnowledgeSource,
                        Version = geoObject.GeoObjectInfo.Version,
                        LanguageCode = geoObject.GeoObjectInfo.LanguageCode,
                        Language = geoObject.GeoObjectInfo.Language,
                        Status = geoObject.GeoObjectInfo.Status,
                        ArchiveTime = geoObject.GeoObjectInfo.ArchiveTime,
                        UpdateTime = geoObject.GeoObjectInfo.UpdateTime,
                        CreationTime = geoObject.GeoObjectInfo.CreationTime,
                        CommonInfo = geoObject.GeoObjectInfo.CommonInfo,
                    };
                    if (geoObject.GeoObjectInfo.Classifiers != null)
                    {
                        geoObjectDTO.GeoObjectInfo.Classifiers = new();
                        foreach (var classifier in geoObject.GeoObjectInfo.Classifiers)
                        {
                            geoObjectDTO.GeoObjectInfo.Classifiers.Add(new ClassifierDTO
                            {
                                Id = classifier.Id,
                                Name = classifier.Name,
                                Code = classifier.Code,
                                CommonInfo = classifier.CommonInfo

                            });
                        }
                    }
                }
                if (geoObject.ParentGeoObjects != null)
                {
                    geoObjectDTO.Aspects = new();
                    foreach (var parentGeoObject in geoObject.ParentGeoObjects)
                    {
                        geoObjectDTO.ParentGeoObjects.Add(new ParentChildObjectLinkDTO
                        {
                            Id = parentGeoObject.Id,
                            ParentGeographicalObjectName = parentGeoObject.ParentGeographicalObjectName,
                            ChildGeographicalObjectName = parentGeoObject.ChildGeographicalObjectName,
                            CompletelyIncludedFlag = parentGeoObject.CompletelyIncludedFlag,
                            IncludedPercent = parentGeoObject.IncludedPercent,
                            CreationDateTime = parentGeoObject.CreationDateTime,
                            LastUpdatedDateTime = parentGeoObject.LastUpdatedDateTime,
                            ParentGeographicalObjectId = parentGeoObject.ParentGeographicalObjectId,
                            ChildGeographicalObjectId = parentGeoObject.ChildGeographicalObjectId
                        });
                    }
                }
                if (geoObject.ChildGeoObjects != null)
                {
                    geoObjectDTO.ChildGeoObjects = new();
                    foreach (var childGeoObject in geoObject.ChildGeoObjects)
                    {
                        geoObjectDTO.ChildGeoObjects.Add(new ParentChildObjectLinkDTO
                        {
                            Id = childGeoObject.Id,
                            ParentGeographicalObjectName = childGeoObject.ParentGeographicalObjectName,
                            ChildGeographicalObjectName = childGeoObject.ChildGeographicalObjectName,
                            CompletelyIncludedFlag = childGeoObject.CompletelyIncludedFlag,
                            IncludedPercent = childGeoObject.IncludedPercent,
                            CreationDateTime = childGeoObject.CreationDateTime,
                            LastUpdatedDateTime = childGeoObject.LastUpdatedDateTime,
                            ParentGeographicalObjectId = childGeoObject.ParentGeographicalObjectId,
                            ChildGeographicalObjectId = childGeoObject.ChildGeographicalObjectId
                        });
                    }
                }
                if (geoObject.OutputTopologyLinks != null)
                {
                    geoObjectDTO.OutputTopologyLinks = new();
                    foreach (var outputTopologyLink in geoObject.OutputTopologyLinks)
                    {
                        geoObjectDTO.OutputTopologyLinks.Add(new TopologyLinkDTO
                        {
                            Id = outputTopologyLink.Id,
                            Predicate = outputTopologyLink.Predicate,
                            Status = outputTopologyLink.Status,
                            CreationDateTime = outputTopologyLink.CreationDateTime,
                            LastUpdatedDateTime = outputTopologyLink.LastUpdatedDateTime,
                            CommonBorder = outputTopologyLink.CommonBorder,
                            GeographicalObjectInId = outputTopologyLink.GeographicalObjectInId,
                            GeographicalObjectOutId = outputTopologyLink.GeographicalObjectOutId
                        });
                    }
                }
                if (geoObject.InputTopologyLinks != null)
                {
                    geoObjectDTO.InputTopologyLinks = new();
                    foreach (var inputTopologyLink in geoObject.InputTopologyLinks)
                    {
                        geoObjectDTO.InputTopologyLinks.Add(new TopologyLinkDTO
                        {
                            Id = inputTopologyLink.Id,
                            Predicate = inputTopologyLink.Predicate,
                            Status = inputTopologyLink.Status,
                            CreationDateTime = inputTopologyLink.CreationDateTime,
                            LastUpdatedDateTime = inputTopologyLink.LastUpdatedDateTime,
                            CommonBorder = inputTopologyLink.CommonBorder,
                            GeographicalObjectInId = inputTopologyLink.GeographicalObjectInId,
                            GeographicalObjectOutId = inputTopologyLink.GeographicalObjectOutId
                        });
                    }
                }

                if (geoObject.Aspects != null)
                {
                    geoObjectDTO.Aspects = new();
                    foreach (var aspect in geoObject.Aspects)
                    {
                        geoObjectDTO.Aspects.Add(new AspectDTO
                        {
                            Id = aspect.Id,
                            Type = aspect.Type,
                            Code = aspect.Code,
                            EndPoint = aspect.EndPoint,
                            CommonInfo = aspect.CommonInfo,
                            GeographicalObjectId = aspect.GeographicalObjectId
                        });
                    }
                }
            });
            return geoObjectDTO;
        }
        public async Task<GeoObject> DTOToObject(GeoObjectDTO geoObjectDTO)
        {
            GeoObject geoObject = new GeoObject();
            await Task.Run(() =>
            {
                geoObject.Id = (Guid)geoObjectDTO.Id;
                geoObject.Name = geoObjectDTO.Name;
                geoObject.GeoNameId = (int?)geoObjectDTO.GeoNameId;
                geoObject.Status = (Status?)geoObjectDTO.Status;
                geoObject.UpdateTime = (DateTime?)geoObjectDTO.UpdateTime;
                geoObject.CreationTime = (DateTime?)geoObjectDTO.CreationTime;
                if (geoObjectDTO.GeoNameFeature != null)
                {
                    geoObject.GeoNameFeature = new GeoNamesFeature
                    {
                        Id = (Guid)geoObjectDTO.GeoNameFeature.Id,
                        GeoNamesFeatureCode = geoObjectDTO.GeoNameFeature.GeoNamesFeatureCode,
                        GeoNamesFeatureKind = geoObjectDTO.GeoNameFeature.GeoNamesFeatureKind,
                        FeatureKindNameEn = geoObjectDTO.GeoNameFeature.FeatureKindNameEn,
                        FeatureNameEn = geoObjectDTO.GeoNameFeature.FeatureNameEn,
                        FeatureKindNameRu = geoObjectDTO.GeoNameFeature.FeatureKindNameRu,
                        FeatureNameRu = geoObjectDTO.GeoNameFeature.FeatureNameRu,
                        CommentsEn = geoObjectDTO.GeoNameFeature.CommentsEn,
                        CommentsRu = geoObjectDTO.GeoNameFeature.CommentsRu,
                    };
                }
                if (geoObjectDTO.GeometryVersion != null)
                {
                    foreach (var geometryVersion in geoObjectDTO.GeometryVersion)
                    {
                        geoObject.GeometryVersion.Add(new GeometryVersion
                        {
                            Id = (Guid)geometryVersion.Id,
                            AuthoritativeKnowledgeSource = geometryVersion.AuthoritativeKnowledgeSource,
                            Version = (int?)geometryVersion.Version,
                            Status = (Status?)geometryVersion.Status,
                            ArchiveTime = (DateTime?)geometryVersion.ArchiveTime,
                            UpdateTime = (DateTime?)geometryVersion.UpdateTime,
                            CreationTime = (DateTime?)geometryVersion.CreationTime,
                            BorderGeocodes = geometryVersion.BorderGeocodes,
                            AreaValue = (double?)geometryVersion.AreaValue,
                            WestToEastLength = (double?)geometryVersion.WestToEastLength,
                            NorthToSouthLength = (double?)geometryVersion.NorthToSouthLength
                        });
                    }
                }
                if (geoObjectDTO.Geometry != null)
                {
                    geoObject.Geometry = new GeometryVersion
                    {
                        Id = (Guid)geoObjectDTO.Geometry.Id,
                        AuthoritativeKnowledgeSource = geoObjectDTO.Geometry.AuthoritativeKnowledgeSource,
                        Version = (int?)geoObjectDTO.Geometry.Version,
                        Status = (Status?)geoObjectDTO.Geometry.Status,
                        ArchiveTime = (DateTime?)geoObjectDTO.Geometry.ArchiveTime,
                        UpdateTime = (DateTime?)geoObjectDTO.Geometry.UpdateTime,
                        CreationTime = (DateTime?)geoObjectDTO.Geometry.CreationTime,
                        BorderGeocodes = geoObjectDTO.Geometry.BorderGeocodes,
                        AreaValue = (double?)geoObjectDTO.Geometry.AreaValue,
                        WestToEastLength = (double?)geoObjectDTO.Geometry.WestToEastLength,
                        NorthToSouthLength = (double?)geoObjectDTO.Geometry.NorthToSouthLength
                    };
                }

                if (geoObjectDTO.GeoObjectInfo != null)
                {
                    geoObject.GeoObjectInfo = new GeoObjectInfo
                    {
                        Id = (Guid)geoObjectDTO.GeoObjectInfo.Id,
                        FullName = geoObjectDTO.GeoObjectInfo.FullName,
                        ShortName = geoObjectDTO.GeoObjectInfo.ShortName,
                        AuthoritativeKnowledgeSource = geoObjectDTO.GeoObjectInfo.AuthoritativeKnowledgeSource,
                        Version = (int?)geoObjectDTO.GeoObjectInfo.Version,
                        LanguageCode = geoObjectDTO.GeoObjectInfo.LanguageCode,
                        Language = geoObjectDTO.GeoObjectInfo.Language,
                        Status = (Status?)geoObjectDTO.GeoObjectInfo.Status,
                        ArchiveTime = (DateTime?)geoObjectDTO.GeoObjectInfo.ArchiveTime,
                        UpdateTime = (DateTime?)geoObjectDTO.GeoObjectInfo.UpdateTime,
                        CreationTime = (DateTime?)geoObjectDTO.GeoObjectInfo.CreationTime,
                        CommonInfo = (String?)geoObjectDTO.GeoObjectInfo.CommonInfo

                    };

                    if (geoObjectDTO.GeoObjectInfo.Classifiers != null)
                    {
                        geoObject.GeoObjectInfo.Classifiers = new();
                        foreach (var classifier in geoObjectDTO.GeoObjectInfo.Classifiers)
                        {
                            geoObject.GeoObjectInfo.Classifiers.Add(new Classifier
                            {
                                Id = (Guid)classifier.Id,
                                Name = (String?)classifier.Name,
                                Code = (String?)classifier.Code,
                                CommonInfo = (String?)classifier.CommonInfo
                            });
                        }
                    }
                }
                if (geoObjectDTO.ParentGeoObjects != null)
                {
                    geoObject.ParentGeoObjects = new();
                    foreach (var parentGeoObject in geoObjectDTO.ParentGeoObjects)
                    {
                        geoObject.ParentGeoObjects.Add(new ParentChildObjectLink
                        {
                            Id = (Guid)parentGeoObject.Id,
                            ParentGeographicalObjectName = parentGeoObject.ParentGeographicalObjectName,
                            ChildGeographicalObjectName = parentGeoObject.ChildGeographicalObjectName,
                            CompletelyIncludedFlag = (bool?)parentGeoObject.CompletelyIncludedFlag,
                            IncludedPercent = (double?)parentGeoObject.IncludedPercent,
                            CreationDateTime = (DateTime?)parentGeoObject.CreationDateTime,
                            LastUpdatedDateTime = (DateTime?)parentGeoObject.LastUpdatedDateTime,
                            ParentGeographicalObjectId = (Guid)parentGeoObject.ParentGeographicalObjectId,
                            ChildGeographicalObjectId = (Guid)parentGeoObject.ChildGeographicalObjectId
                        });
                    }
                }
                if (geoObjectDTO.ChildGeoObjects != null)
                {
                    geoObject.ChildGeoObjects = new();
                    foreach (var childGeoObject in geoObjectDTO.ChildGeoObjects)
                    {
                        geoObject.ChildGeoObjects.Add(new ParentChildObjectLink
                        {
                            Id = (Guid)childGeoObject.Id,
                            ParentGeographicalObjectName = childGeoObject.ParentGeographicalObjectName,
                            ChildGeographicalObjectName = childGeoObject.ChildGeographicalObjectName,
                            CompletelyIncludedFlag = (bool?)childGeoObject.CompletelyIncludedFlag,
                            IncludedPercent = (double?)childGeoObject.IncludedPercent,
                            CreationDateTime = (DateTime?)childGeoObject.CreationDateTime,
                            LastUpdatedDateTime = (DateTime?)childGeoObject.LastUpdatedDateTime,
                            ParentGeographicalObjectId = (Guid)childGeoObject.ParentGeographicalObjectId,
                            ChildGeographicalObjectId = (Guid)childGeoObject.ChildGeographicalObjectId
                        });
                    }
                }
                if (geoObjectDTO.OutputTopologyLinks != null)
                {
                    geoObject.OutputTopologyLinks = new();
                    foreach (var outputTopologyLink in geoObjectDTO.OutputTopologyLinks)
                    {
                        geoObject.OutputTopologyLinks.Add(new TopologyLink
                        {
                            Id = (Guid)outputTopologyLink.Id,
                            Predicate = outputTopologyLink.Predicate,
                            Status = (Status?)outputTopologyLink.Status,
                            CreationDateTime = (DateTime?)outputTopologyLink.CreationDateTime,
                            LastUpdatedDateTime = (DateTime?)outputTopologyLink.LastUpdatedDateTime,
                            CommonBorder = outputTopologyLink.CommonBorder
                        });
                    }
                }
                if (geoObjectDTO.InputTopologyLinks != null)
                {
                    geoObject.InputTopologyLinks = new();
                    foreach (var inputTopologyLink in geoObjectDTO.InputTopologyLinks)
                    {
                        geoObject.InputTopologyLinks.Add(new TopologyLink
                        {
                            Id = (Guid)inputTopologyLink.Id,
                            Predicate = inputTopologyLink.Predicate,
                            Status = (Status?)inputTopologyLink.Status,
                            CreationDateTime = (DateTime?)inputTopologyLink.CreationDateTime,
                            LastUpdatedDateTime = (DateTime?)inputTopologyLink.LastUpdatedDateTime,
                            CommonBorder = inputTopologyLink.CommonBorder
                        });
                    }
                }

                if (geoObjectDTO.Aspects != null)
                {
                    geoObject.Aspects = new();
                    foreach (var aspect in geoObjectDTO.Aspects)
                    {
                        geoObject.Aspects.Add(new Aspect
                        {
                            Id = (Guid)aspect.Id,
                            Type = aspect.Type,
                            Code = aspect.Code,
                            EndPoint = aspect.EndPoint,
                            CommonInfo = aspect.CommonInfo,
                            GeographicalObjectId = aspect.GeographicalObjectId
                        });
                    }
                }
            });
            return geoObject;
        }
    }
}
