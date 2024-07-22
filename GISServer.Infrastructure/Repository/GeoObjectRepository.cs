using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GISServer.Infrastructure.Service
{
    public class GeoObjectRepository : IGeoObjectRepository
    {
        private readonly Context _context;
        private readonly IGeoObjectsClassifiersRepository _gocRepository;

        public GeoObjectRepository(Context context, IGeoObjectsClassifiersRepository gocRepository)
        {
            _context = context;
            _gocRepository = gocRepository;
        }

        public async Task<List<GeoObject>> GetGeoObjects()
        {
            return await _context.GeoObjects
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
        }

        public async Task<GeoObject> GetGeoObject(Guid id)
        {
            return await _context.GeoObjects
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
        }
        public async Task<GeoObject> GetByNameAsync(string name)
        {
            return await _context.GeoObjects
                .Where(o => o.Name == name)
                .Include(l => l.InputTopologyLinks)
                .Include(l2 => l2.OutputTopologyLinks)
                .FirstOrDefaultAsync();
        }

        // GeoObjectsClassifiersRepository
        //public async Task<List<GeoObjectsClassifiers>> GetClassifiers(Guid geoObjectId)
        //{
            //return await _context.GeoObjectsClassifiers
                //.Where(o => o.GeoObjectId == geoObjectId)
                //.ToListAsync();
        //}

        public async Task<List<GeoObjectsClassifiers>> GetGeoObjectsClassifiers(Guid? geoObjectInfoId)
        {
            return await _context.GeoObjectsClassifiers
                .Where(o=>o.GeoObjectId == geoObjectInfoId)
                .Include(gogc => gogc.GeoObjectInfo)
                .Include(gogc => gogc.Classifier)
                .ToListAsync();
        }
        
        public void ChangeTrackerClear()
        {
            _context.ChangeTracker.Clear();
        }

        public async Task<GeoObject> AddGeoObject(GeoObject geoObject)
        {

            await _context.GeoObjects.AddAsync(geoObject);
            await _context.SaveChangesAsync();
            return await GetGeoObject(geoObject.Id);
        }

        public async Task<List<GeoObjectsClassifiers>> AddGeoObjectsClassifiers(GeoObjectsClassifiers geoObjectsClassifiers)
        {
            await _context.GeoObjectsClassifiers.AddAsync(geoObjectsClassifiers);
            await _context.SaveChangesAsync();
            return await GetGeoObjectsClassifiers(geoObjectsClassifiers.GeoObjectId);
        }

        public async Task<GeoObject> UpdateGeoObject(GeoObject geoObject)
        {

            _context.Entry(geoObject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return geoObject;
        }

        public async Task UpdateAsync(GeoObject geoObject)
        {
            var existgeoObject = GetGeoObject(geoObject.Id).Result;
            if (existgeoObject != null)
            {
                _context.Entry(existgeoObject).CurrentValues.SetValues(geoObject);

                ////////// InputTopologyLinks ///////////
                Console.WriteLine("\n\n////////// InputTopologyLinks ///////////");
                foreach (var InputTopologylink in geoObject.InputTopologyLinks)
                {
                    var existInputTopologylink = existgeoObject.InputTopologyLinks.FirstOrDefault(l => l.Id == InputTopologylink.Id);
                    if (existInputTopologylink == null)
                    {
                        existgeoObject.InputTopologyLinks.Add(InputTopologylink);
                    }
                    else
                    {
                        _context.Entry(existInputTopologylink).CurrentValues.SetValues(InputTopologylink);
                    }
                }
                foreach (var existInputlink in existgeoObject.InputTopologyLinks)
                {
                    if (!geoObject.InputTopologyLinks.Any(l => l.Id == existInputlink.Id))
                    {
                        _context.Remove(existInputlink);
                    }
                }
                ////////// InputTopologyLinks ///////////
                Console.WriteLine("\n\n////////// OutputTopologyLinks ///////////");
                foreach (var OutputTopologylink in geoObject.OutputTopologyLinks)
                {
                    var existOutputTopologylink = existgeoObject.OutputTopologyLinks.FirstOrDefault(l => l.Id == OutputTopologylink.Id);
                    if (existOutputTopologylink == null)
                    {
                        existgeoObject.OutputTopologyLinks.Add(OutputTopologylink);
                    }
                    else
                    {
                        _context.Entry(existOutputTopologylink).CurrentValues.SetValues(OutputTopologylink);
                    }
                }
                foreach (var existOutputlink in existgeoObject.OutputTopologyLinks)
                {
                    if (!geoObject.OutputTopologyLinks.Any(l => l.Id == existOutputlink.Id))
                    {
                        _context.Remove(existOutputlink);
                    }
                }
                ////////// Classifier ///////////
                Console.WriteLine("\n\n////////// Classifier ///////////");
                foreach (var classifier in geoObject.GeoObjectInfo.Classifiers)
                {
                    var existClassifier = existgeoObject.GeoObjectInfo.Classifiers.FirstOrDefault(l => l.Id == classifier.Id);
                    if (existClassifier == null)
                    {
                        existgeoObject.GeoObjectInfo.Classifiers.Add(classifier);
                    }
                    else
                    {
                        _context.Entry(existClassifier).CurrentValues.SetValues(classifier);
                    }
                }
                foreach (var existClassifier in existgeoObject.GeoObjectInfo.Classifiers)
                {
                    if (!geoObject.GeoObjectInfo.Classifiers.Any(l => l.Id == existClassifier.Id))
                    {
                        _context.Remove(existClassifier);
                    }
                }
                ////////// Aspect ///////////
                Console.WriteLine("\n\n////////// Aspect ///////////");
                foreach (var aspect in geoObject.Aspects)
                {
                    var existAspect = existgeoObject.Aspects.FirstOrDefault(l => l.Id == aspect.Id);
                    if (existAspect == null)
                    {
                        existgeoObject.Aspects.Add(aspect);
                    }
                    else
                    {
                        _context.Entry(existAspect).CurrentValues.SetValues(aspect);
                    }
                }
                foreach (var existAspect in existgeoObject.Aspects)
                {
                    if (!geoObject.Aspects.Any(l => l.Id == existAspect.Id))
                    {
                        _context.Remove(existAspect);
                    }
                }
                ////////// ParentChild ///////////
                Console.WriteLine("\n\n////////// ParentChild ///////////");
                foreach (var parent in geoObject.ParentGeoObjects)
                {
                    var existParent = existgeoObject.ParentGeoObjects.FirstOrDefault(l => l.Id == parent.Id);
                    if (existParent == null)
                    {
                        existgeoObject.ParentGeoObjects.Add(parent);
                    }
                    else
                    {
                        _context.Entry(existParent).CurrentValues.SetValues(parent);
                    }
                }
                foreach (var existParent in existgeoObject.ParentGeoObjects)
                {
                    if (!geoObject.ParentGeoObjects.Any(l => l.Id == existParent.Id))
                    {
                        _context.Remove(existParent);
                    }
                }
                foreach (var child in geoObject.ChildGeoObjects)
                {
                    var existChild = existgeoObject.ChildGeoObjects.FirstOrDefault(l => l.Id == child.Id);
                    if (existChild == null)
                    {
                        existgeoObject.ChildGeoObjects.Add(child);
                    }
                    else
                    {
                        _context.Entry(existChild).CurrentValues.SetValues(child);
                    }
                }
                foreach (var existChild in existgeoObject.ChildGeoObjects)
                {
                    if (!geoObject.ChildGeoObjects.Any(l => l.Id == existChild.Id))
                    {
                        _context.Remove(existChild);
                    }
                }
            }
            await _context.SaveChangesAsync();
        }

            public async Task<(bool, string)> DeleteGeoObject(Guid id)
            {

                var dbGeoObject = await GetGeoObject(id);
                if (dbGeoObject == null)
                {
                    return (false, "GeoObeject could not be found");
                }
                //TpologyLinks
                foreach (var inputTopologyLink in dbGeoObject.InputTopologyLinks)
                {
                    _context.TopologyLinks.Remove(inputTopologyLink);
                }
                foreach (var outputTopologyLink in dbGeoObject.OutputTopologyLinks)
                {
                    _context.TopologyLinks.Remove(outputTopologyLink);
                }
                //ParentChildLinks
                foreach(var parent in dbGeoObject.ParentGeoObjects)
                {
                    _context.ParentChildObjectLinks.Remove(parent);
                }
                foreach (var child in dbGeoObject.ChildGeoObjects)
                {
                    _context.ParentChildObjectLinks.Remove(child);
                }
                _context.GeoObjects.Remove(dbGeoObject);
                await _context.SaveChangesAsync();
                return (true, "GeoObject got deleted");
            }

        public async Task<(bool, string)> Archive(Guid id)
        {
            var dbGeoObject = await GetGeoObject(id);

            if (dbGeoObject == null)
            {
                return (false, "GeoObeject could not be found");
            }
            dbGeoObject.Status = Status.Archive;
            await _context.SaveChangesAsync();
            return (true, "GeoObject got archived");
        }

        public async Task<GeoObject> AddGeoObjectAspect(Guid geoObjectId, Guid aspectId)
        {
            _context.Aspects
                .Where(a => a.Id == aspectId)
                .ExecuteUpdate(b =>
                    b.SetProperty(a => a.GeographicalObjectId, geoObjectId)
                );
            return await GetGeoObject(geoObjectId);
        }

        public async Task<List<Aspect>> GetGeoObjectAspects(Guid geoObjectId)
        {
            return await _context.Aspects
                .Where(a => a.GeographicalObjectId == geoObjectId)
                .ToListAsync();
        }
    }
}
