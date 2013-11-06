cd Core
call nugetPack.cmd

cd ..
cd Client
call nugetPack.cmd

cd ..
cd EmbeddedSts
call NugetPack.cmd

cd ..
cd WCF
call NugetPack.cmd

cd ..
cd WebAPI
call NugetPack.cmd

cd ..
cd WebAPI.AuthenticationHandler
call NugetPack.cmd

cd ..
cd Hawk
call NugetPack.cmd

cd ..
cd SystemWeb
call NugetPack.cmd