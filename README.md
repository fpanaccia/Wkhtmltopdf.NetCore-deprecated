# Wkhtmltopdf.NetCore

This project implements the library wkhtmltopdf for net core, working in windows and linux and docker.

For more information about how to use it, go to https://github.com/fpanaccia/Wkhtmltopdf.NetCore.Example

# I dont want too see another repository

Just simply add to your Startup.cs in ConfigureServices method, this line "Wkhtmltopdf.NetCore.RotativaConfiguration.Setup();", like this

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Wkhtmltopdf.NetCore.RotativaConfiguration.Setup();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        
If you are using the docker container for net core provided from microsoft, you need to add this line to the dockerfile "RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev", like this

        FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
        WORKDIR /app
        RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev
        EXPOSE 80

In linux you also need to install the same libraries used in the dockerfile, they are libgdiplus, libc6-dev
