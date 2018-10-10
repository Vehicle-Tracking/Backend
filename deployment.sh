#! /bin/bash

APPLICATION_ENVIRONMENT=$1
PROFILE=$2

set -x
set -e

apk add curl

curl "https://s3.amazonaws.com/aws-cli/awscli-bundle.zip" -o "awscli-bundle.zip"        
unzip awscli-bundle.zip
./awscli-bundle/install -b ~/bin/aws
export PATH=~/bin:$PATH
curl -O https://bootstrap.pypa.io/get-pip.py
python get-pip.py

pip install --upgrade pip

pip install --upgrade botocore
pip install --upgrade awscli 
pip install --upgrade awsebcli 

aws --version
eb --version

echo "*** preconfiguring environment ... ***"  

aws configure set aws_access_key_id $AWS_ACCESS_KEY_ID
aws configure set aws_secret_access_key $AWS_SECRET_ACCESS_KEY
aws configure set default.region $AWS_REGION

echo "*** deployment started ... ***"  

eb init "${APPLICATION_NAME}" --platform "arn:aws:elasticbeanstalk:${AWS_REGION}::platform/Docker running on 64bit Amazon Linux/2.12.2" --region $AWS_REGION
eb use $APPLICATION_ENVIRONMENT
eb deploy $APPLICATION_ENVIRONMENT 
