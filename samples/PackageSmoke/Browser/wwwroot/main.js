import { dotnet } from './_framework/dotnet.js';

const { getConfig, runMain } = await dotnet.create();
await runMain(getConfig().mainAssemblyName, []);
