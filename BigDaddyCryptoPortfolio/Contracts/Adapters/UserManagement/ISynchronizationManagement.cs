﻿using BigDaddyCryptoPortfolio.Models.Api;
using BigDaddyCryptoPortfolio.Models.Dtos;
using BigDaddyCryptoPortfolio.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement
{
    public interface ISynchronizationManagement
    {
        public ApiResult<SynchronizationResponse> BeginCommit(User user);
        public void Push<T>(SynchronizationTask<T> action);
        public ApiResult<SynchronizationResponse> EndCommit();
    }
}
