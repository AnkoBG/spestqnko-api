﻿using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spestqnko.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context)
            : base(context)
        { }
    }
}
