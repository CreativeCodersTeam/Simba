﻿using Simba.Server.Core;

await SimbaDaemonHostBuilder.CreateSimbaDaemonHostBuilder(args)
    .Build()
    .RunAsync()
    .ConfigureAwait(false);
