﻿using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GISServer.Infrastructure.Service
{
    public class GeoObjectRepository : IGeoObjectRepository
    {
        private readonly Context _context;

        public GeoObjectRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<GeoObject>> Get()
        {
            var query = await _context.GeoObjects
                .Include(gnf => gnf.GeoNameFeature)
                .Include(gv => gv.GeometryVersion)
                .Include(g => g.Geometry)
                .Include(goi => goi.GeoObjectInfo)
                .Include(pgo => pgo.ParentGeoObjects)
                .Include(cgo => cgo.ChildGeoObjects)
                .Include(itl => itl.InputTopologyLinks)
                .Include(otl => otl.OutputTopologyLinks)
                .Include(a => a.Aspects)
                .ToListAsync();
            if (query is not null) return query;
            return null;
        }

        public async Task<GeoObject> Get(Guid id)
        {
            var query = await _context.GeoObjects
                .Where(go => go.Id == id)
                .Include(gnf => gnf.GeoNameFeature)
                .Include(gv => gv.GeometryVersion)
                .Include(g => g.Geometry)
                .Include(goi => goi.GeoObjectInfo)
                .Include(pgo => pgo.ParentGeoObjects)
                .Include(cgo => cgo.ChildGeoObjects)
                .Include(itl => itl.InputTopologyLinks)
                .Include(otl => otl.OutputTopologyLinks)
                .Include(a => a.Aspects)
                .FirstOrDefaultAsync();
            if (query is not null) return query;
            return null;
        }
        public async Task<GeoObject> GetByNameAsync(string name)
        {
            var query = await _context.GeoObjects
                .Where(o => o.Name == name)
                .Include(itl => itl.InputTopologyLinks)
                .Include(otl => otl.OutputTopologyLinks)
                .FirstOrDefaultAsync();
            if (query is not null) return query;
            return null;
        }

        public async Task<List<GeoObjectsClassifiers>> GetClassifiers(Guid? geoObjectInfoId)
        {
            var query = await _context.GeoObjectsClassifiers
                .Where(o => o.GeoObjectId == geoObjectInfoId)
                .Include(gogc => gogc.GeoObjectInfo)
                .Include(gogc => gogc.Classifier)
                .ToListAsync();
            if (query is not null) return query;
            return null;
        }

        public void ChangeTrackerClear()
        {
            _context.ChangeTracker.Clear();
        }

        public async Task<GeoObject> Add(GeoObject geoObject)
        {

            await _context.GeoObjects.AddAsync(geoObject);
            await _context.SaveChangesAsync();
            return await Get(geoObject.Id);
        }

        public async Task<List<GeoObjectsClassifiers>> AddClassifier(GeoObjectsClassifiers geoObjectsClassifiers)
        {
            await _context.GeoObjectsClassifiers.AddAsync(geoObjectsClassifiers);
            await _context.SaveChangesAsync();
            return await GetClassifiers(geoObjectsClassifiers.GeoObjectId);
        }

        public async Task AddClassifier(Guid geoObjectId, Guid classifierId)
        {
            var goc = new GeoObjectsClassifiers
            {
                ClassifierId = classifierId,
                GeoObjectId = geoObjectId,
                Status = Status.Actual
            };
            await AddClassifier(goc);   
        }

        // public async Task<GeoObject> UpdateGeoObject(GeoObject geoObject)
        // {

        //     _context.Entry(geoObject).State = EntityState.Modified;
        //     await _context.SaveChangesAsync();
        //     return geoObject;
        // }

        public async Task UpdateAsync(GeoObject geoObject)
        {
            var existgeoObject = Get(geoObject.Id).Result;
            if (existgeoObject != null)
            {
                _context.Entry(existgeoObject).CurrentValues.SetValues(geoObject);

                if (geoObject.InputTopologyLinks != null)
                {
                    foreach (var InputTopologylink in geoObject.InputTopologyLinks)
                    {
                        var existInputTopologylink = existgeoObject.InputTopologyLinks!.FirstOrDefault(l => l.Id == InputTopologylink.Id);
                        if (existInputTopologylink == null)
                        {
                            existgeoObject.InputTopologyLinks!.Add(InputTopologylink);
                        }
                        else
                        {
                            _context.Entry(existInputTopologylink).CurrentValues.SetValues(InputTopologylink);
                        }
                    }
                    foreach (var existInputlink in existgeoObject.InputTopologyLinks!)
                    {
                        if (!geoObject.InputTopologyLinks.Any(l => l.Id == existInputlink.Id))
                        {
                            _context.Remove(existInputlink);
                        }
                    }
                }

                if (geoObject.OutputTopologyLinks != null)
                {
                    foreach (var OutputTopologylink in geoObject.OutputTopologyLinks)
                    {
                        var existOutputTopologylink = existgeoObject.OutputTopologyLinks!.FirstOrDefault(l => l.Id == OutputTopologylink.Id);
                        if (existOutputTopologylink == null)
                        {
                            existgeoObject.OutputTopologyLinks!.Add(OutputTopologylink);
                        }
                        else
                        {
                            _context.Entry(existOutputTopologylink).CurrentValues.SetValues(OutputTopologylink);
                        }
                    }
                    foreach (var existOutputlink in existgeoObject.OutputTopologyLinks!)
                    {
                        if (!geoObject.OutputTopologyLinks.Any(l => l.Id == existOutputlink.Id))
                        {
                            _context.Remove(existOutputlink);
                        }
                    }
                }

                if (geoObject.GeoObjectInfo != null)
                {
                    foreach (var classifier in geoObject.GeoObjectInfo.Classifiers!)
                    {
                        var existClassifier = existgeoObject.GeoObjectInfo!.Classifiers!.FirstOrDefault(l => l.Id == classifier.Id);
                        if (existClassifier == null)
                        {
                            existgeoObject.GeoObjectInfo.Classifiers!.Add(classifier);
                        }
                        else
                        {
                            _context.Entry(existClassifier).CurrentValues.SetValues(classifier);
                        }
                    }
                    if (existgeoObject.GeoObjectInfo != null)
                    {
                        foreach (var existClassifier in existgeoObject.GeoObjectInfo.Classifiers!)
                        {
                            if (!geoObject.GeoObjectInfo.Classifiers.Any(l => l.Id == existClassifier.Id))
                            {
                                _context.Remove(existClassifier);
                            }
                        }
                    }
                }

                if (geoObject.Aspects != null)
                {
                    foreach (var aspect in geoObject.Aspects)
                    {
                        var existAspect = existgeoObject.Aspects!.FirstOrDefault(l => l.Id == aspect.Id);
                        if (existAspect == null)
                        {
                            existgeoObject.Aspects!.Add(aspect);
                        }
                        else
                        {
                            _context.Entry(existAspect).CurrentValues.SetValues(aspect);
                        }
                    }
                    foreach (var existAspect in existgeoObject.Aspects!)
                    {
                        if (!geoObject.Aspects.Any(l => l.Id == existAspect.Id))
                        {
                            _context.Remove(existAspect);
                        }
                    }
                }

                if (geoObject.ParentGeoObjects != null)
                {
                    foreach (var parent in geoObject.ParentGeoObjects)
                    {
                        var existParent = existgeoObject.ParentGeoObjects!.FirstOrDefault(l => l.Id == parent.Id);
                        if (existParent == null)
                        {
                            existgeoObject.ParentGeoObjects!.Add(parent);
                        }
                        else
                        {
                            _context.Entry(existParent).CurrentValues.SetValues(parent);
                        }
                    }
                    foreach (var existParent in existgeoObject.ParentGeoObjects!)
                    {
                        if (!geoObject.ParentGeoObjects.Any(l => l.Id == existParent.Id))
                        {
                            _context.Remove(existParent);
                        }
                    }
                }

                if (geoObject.ChildGeoObjects != null)
                {
                    foreach (var child in geoObject.ChildGeoObjects)
                    {
                        var existChild = existgeoObject.ChildGeoObjects!.FirstOrDefault(l => l.Id == child.Id);
                        if (existChild == null)
                        {
                            existgeoObject.ChildGeoObjects!.Add(child);
                        }
                        else
                        {
                            _context.Entry(existChild).CurrentValues.SetValues(child);
                        }
                    }
                    if (existgeoObject.ChildGeoObjects != null)
                    {
                        foreach (var existChild in existgeoObject.ChildGeoObjects)
                        {
                            if (!geoObject.ChildGeoObjects.Any(l => l.Id == existChild.Id))
                            {
                                _context.Remove(existChild);
                            }
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<(bool, string)> Delete(Guid id)
        {

            var dbGeoObject = await Get(id);
            if (dbGeoObject == null)
            {
                return (false, "GeoObeject could not be found");
            }
            //TpologyLinks
            foreach (var inputTopologyLink in dbGeoObject.InputTopologyLinks!)
            {
                _context.TopologyLinks.Remove(inputTopologyLink);
            }
            foreach (var outputTopologyLink in dbGeoObject.OutputTopologyLinks!)
            {
                _context.TopologyLinks.Remove(outputTopologyLink);
            }
            //ParentChildLinks
            foreach (var parent in dbGeoObject.ParentGeoObjects!)
            {
                _context.ParentChildObjectLinks.Remove(parent);
            }
            foreach (var child in dbGeoObject.ChildGeoObjects!)
            {
                _context.ParentChildObjectLinks.Remove(child);
            }
            _context.GeoObjects.Remove(dbGeoObject);
            await _context.SaveChangesAsync();
            return (true, "GeoObject got deleted");
        }

        public async Task<(bool, string)> Archive(Guid id)
        {
            var dbGeoObject = await Get(id);

            if (dbGeoObject == null)
            {
                return (false, "GeoObeject could not be found");
            }
            dbGeoObject.Status = Status.Archive;
            await _context.SaveChangesAsync();
            return (true, "GeoObject got archived");
        }

        public async Task<GeoObject> AddAspect(Guid geoObjectId, Guid aspectId)
        {
            _context.Aspects
                .Where(a => a.Id == aspectId)
                .ExecuteUpdate(b =>
                    b.SetProperty(a => a.GeographicalObjectId, geoObjectId)
                );
            return await Get(geoObjectId);
        }

        public async Task<List<Aspect>> GetAspects(Guid geoObjectId)
        {
            return await _context.Aspects
                .Where(a => a.GeographicalObjectId == geoObjectId)
                .ToListAsync();
        }
    }
}
