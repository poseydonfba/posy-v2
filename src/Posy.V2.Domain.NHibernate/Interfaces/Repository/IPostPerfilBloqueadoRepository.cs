﻿using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IPostPerfilBloqueadoRepository : IRepository<PostPerfilBloqueado, int>
    {
        PostPerfilBloqueado Get(int usuarioId, int usuarioIdBloqueado);
    }
}
