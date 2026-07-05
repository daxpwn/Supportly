using Supportly.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Implementation.UseCases
{
    public abstract class EfUseCase
    {
        protected readonly LabDbContext ctx;

        protected EfUseCase(LabDbContext context)
        {
            ctx = context;
        }
    }
}
