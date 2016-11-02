using CamajanSport.BOL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ApiCamajan.Context
{
    public class CamajanSportContext: DbContext
    {
        public CamajanSportContext() : base ("name=dbCamajan") {}

        public DbSet<Usuario> usuarios { get; set; }

        public DbSet<Deporte> deportes { get; set; }

        public DbSet<Rol> roles { get; set; }
    }
}