#Google Cloud node js functions initialization

## Steps to do in console
1. Create project
2. Enable Funtions
3. Create Storage Bucket and enable APIs
4. Add IAM permission to service account to manage storage
in my case it was IAM and admin -> IAM -> Permssions -> Add -> AppEngine default service account add role Storage Admin 
You could create a more fine graned role 
It should be able to read buckets and manage objects in the bucket. You could also manage permissions for a specific bucket if you go to bucket details -> permissions

```
gcloud projects list
gcloud config set project <project-name>
gcloud functions deploy helloContent --runtime nodejs12 --trigger-http --allow-unauthenticated
gcloud functions describe helloContent
```

## Create access keys for Raspberry command service to read the command file

```
gcloud iam service-accounts create raspberry-lightswitcher --description="Service account for Home Raspberry LightSwitcher" --display-name="Raspberry LightSwitcher"
gcloud projects add-iam-policy-binding <project-id> --member="serviceAccount:raspberry-lightswitcher@<project-id>.iam.gserviceaccount.com" --role="roles/storage.objectAdmin"
gcloud iam service-accounts keys create gcloud_service_account_key.json --iam-account=raspberry-lightswitcher@<project-id>.iam.gserviceaccount.com
```
