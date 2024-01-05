using System;

namespace Template.Domain.Interfaces.UoW;

public interface IUnitOfWork : IDisposable
{
    int Commit();
    void BeginTransaction();
    void BeginCommit();
    void BeginRollback();
}
