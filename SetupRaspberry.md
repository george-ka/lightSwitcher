
# Setting up Raspberry PI

You will need to install .net core on raspberry 
https://edi.wang/post/2019/9/29/setup-net-core-30-runtime-and-sdk-on-raspberry-pi-4

You will also need to install arduino cli to be able to upload program to Arduino.

```bash
sudo apt-get update
sudo apt-get upgrade

# Download dotnet core 3.1 SDK
wget https://download.visualstudio.microsoft.com/download/pr/349f13f0-400e-476c-ba10-fe284b35b932/44a5863469051c5cf103129f1423ddb8/dotnet-sdk-3.1.102-linux-arm.tar.gz


mkdir dotnet
tar zxf dotnet-sdk-3.1.102-linux-arm.tar.gz -C $HOME/dotnet

```

# "Auto Start" .NET Core Environment

Every time you restart your Raspberry Pi, you'll have to re-configure the DOTNET_ROOT and PATH environment variables or .NET CLI won't work. To make them auto start with the system, we can modify the ".profile".

```bash
cd ~
sudo nano .profile
```

Add those lines at the end of this file

```bash
# set .NET Core SDK and Runtime path
export DOTNET_ROOT=$HOME/dotnet
export PATH=$PATH:$HOME/dotnet
```

Install and start Nginx

```bash
sudo apt-get install nginx
sudo /etc/init.d/nginx start
```

Open Nginx config file:

```bash
sudo nano /etc/nginx/sites-available/default
```

Replace its content with:
```
server {
    listen        80 default_server;
    server_name   _;
    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
}
``` 

Check and apply the config file:

```
sudo nginx -t
sudo nginx -s reload
```
