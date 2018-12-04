using MyScullion.Models;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyScullion.Services.Databases.Realm
{
    public class WrapRealm<T> : RealmObject where T : BaseModel
    {
        public WrapRealm()
        {

        }

        public WrapRealm(T model)
        {
            Model = model;
        }

        public T Model { get; set; }

        [PrimaryKey]
        public int Id => Model.Id;
    }
}
