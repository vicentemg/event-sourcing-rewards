FROM mcr.microsoft.com/devcontainers/dotnet:1-9.0-bookworm

# Install oh-my-posh
RUN wget https://github.com/JanDeDobbeleer/oh-my-posh/releases/latest/download/posh-linux-amd64 -O /usr/local/bin/oh-my-posh \
    && chmod +x /usr/local/bin/oh-my-posh

# Install dependencies for oh-my-posh font install
RUN apt-get update \
    && apt-get install -y curl unzip fontconfig fzf bash-completion

# Copy local .bashrc and fzf into the container from the mounted home directory at runtime
# These will be mounted as volumes in docker-compose.yml
# [Optional] Uncomment this section to install additional OS packages.
# RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
#     && apt-get -y install --no-install-recommends <your-package-list-here>

# [Optional] Uncomment this line to install global node packages.
# RUN su vscode -c "source /usr/local/share/nvm/nvm.sh && npm install -g <your-package-here>" 2>&1