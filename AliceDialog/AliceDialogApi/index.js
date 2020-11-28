'use strict';

const {Storage} = require('@google-cloud/storage');
const storage = new Storage();
// Create this bucket in console upfront
// Setup permission for service account to access the bucket
const myBucket = storage.bucket('alice-light-swithcer-share');
var initializationFailed = false;
var bucketDoesntExist = false;

myBucket.exists((error, exists) =>
{
  if (error)
  {
    console.log(error);
    initializationFailed = true;
    return;
  }

  if (exists) 
  {
    return;
  }

  bucketDoesntExist = true;
});

exports.helloContent = (req, res) => {

  if (typeof req.body.request == "undefined")
  {
    req.body.request = { command : null }
  }

  console.log("Input:" + req.body.request.command + " " + JSON.stringify(req.body)); 
  
  var createResponse = function(sayWhat, endSession)
  {
    return {
      "response": {
          "text": sayWhat,
          "end_session": endSession
        },
        "version": "1.0"
    }
  };

  var voiceCommand = req.body.request.command;
  var outputCommand = null;

  if (voiceCommand == "")
  {
    res.status(200).json(createResponse("Слушаю вашу команду", false));
    return;
  }
  else if (voiceCommand == "включи свет")
  {
    outputCommand = "0";
  } 
  else if (voiceCommand == "выключи свет")
  {
    outputCommand = "8";
  } 
  else
  {
    
  }

  function returnError(error)
  { 
    console.log(error);
    res.status(200).json(createResponse("Произошла неведомая ошибка", true));
  } 

  if (initializationFailed)
  {
    returnError("Initialization failed");
    return;
  }

  if (bucketDoesntExist)
  {
    returnError("Bucket doesn't exist");
    return;
  }

  if (outputCommand === null)
  {
    res.status(200).json(createResponse("Ok", true));
    return;
  }

  var file = myBucket.file("command.json");
  file.save(outputCommand, (error) =>
  {
    if (!error)
    {
      res.status(200).json(createResponse("Ok", true));
    }
    else
    {
      returnError("Error saving file " + JSON.stringify(error));
    }
  })
  
};