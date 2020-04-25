# build docker container locally, because google cloud build doesn't work with MSBuild long file paths
# gcloud builds submit --tag gcr.io/lightswitcher-alice-api/alicedialogapi

docker build -t gcr.io/lightswitcher-alice-api/alicedialogapi:1.0 .

# now we need to push the container to gcloud
# one-time operation to link gcloud and docker
# gcloud auth configure-docker

# push to gcloud
docker push gcr.io/lightswitcher-alice-api/alicedialogapi

gcloud run deploy --image gcr.io/lightswitcher-alice-api/alicedialogapi:1.0 --platform managed
