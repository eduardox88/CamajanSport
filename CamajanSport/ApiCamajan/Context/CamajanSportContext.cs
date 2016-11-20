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

        public DbSet<Pais> paises { get; set; }

        public DbSet<Estado> estados { get; set; }

        public System.Data.Entity.DbSet<CamajanSport.BOL.Equipo> Equipoes { get; set; }

        public System.Data.Entity.DbSet<CamajanSport.BOL.Publicacion> Publicacions { get; set; }
    }
}