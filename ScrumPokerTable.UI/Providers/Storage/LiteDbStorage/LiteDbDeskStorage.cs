using LiteDB;
using ScrumPokerTable.UI.Providers.Exceptions;
using ScrumPokerTable.UI.Providers.Storage.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ScrumPokerTable.UI.Providers.Storage.LiteDbStorage
{
    public class LiteDbDeskStorage : IDeskStorage
    {
        static LiteDbDeskStorage()
        {
            BsonMapper.Global.Entity<DeskEntity>().Id(e => e.Name);
        }

        public LiteDbDeskStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateDesk(DeskEntity desk)
        {
            if (IsDeskExists(desk.Name))
            {
                throw new DeskLogicException(string.Format("Desk {0} already exists", desk.Name));
            }

            using (var db = new LiteDatabase(_connectionString))
            {
                db.GetCollection<DeskEntity>("desk").Insert(desk);
            }
        }

        public void DeleteDesk(DeskEntity desk)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var count = db.GetCollection<DeskEntity>("desk").Delete(d => d.Name == desk.Name);
                
                if (count != 1)
                {
                    throw new DeskNotFoundException(string.Format("Desk {0} not found", desk.Name));
                }
            }
        }

        public DeskEntity GetDesk(string deskName)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var desk = db.GetCollection<DeskEntity>("desk").Find(d => d.Name == deskName).SingleOrDefault();
                if (desk == null)
                {
                    throw new DeskNotFoundException(string.Format("Desk {0} not found", deskName));
                }
                return desk;
            }
        }

        public IReadOnlyCollection<DeskEntity> GetDesks()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<DeskEntity>("desk").FindAll().ToList();
            }
        }

        public bool IsDeskExists(string deskName)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<DeskEntity>("desk").Find(d => d.Name == deskName).Any();
            }
        }

        public void UpdateDesk(DeskEntity desk)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                db.GetCollection<DeskEntity>("desk").Update(desk);
            }
        }

        private readonly string _connectionString;
    }
}