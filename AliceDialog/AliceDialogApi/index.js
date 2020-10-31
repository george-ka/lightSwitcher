'use strict';

const {Storage} = require('@google-cloud/storage');
const storage = new Storage();
// Create this bucket in console upfront
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


/**
 * 
 * @param {File} file 
 */
async function getMetadata(file, res, content)
{
  content.metadata = await file.getMetadata();
  res
      .status(200)
      .json(content);
};

exports.helloContent = (req, res) => {
  
  var createResponse = function(sayWhat)
  {
    return {
      "response": {
          "text": sayWhat,
          "end_session": true
        },
        "version": "1.0"
    }
  };

  function returnError(error)
  { 
    console.log(error);
    res.status(200).json(createResponse("Произошла неведомая ошибка"));
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

  var file = myBucket.file("command.json");
  file.save('{"command": "turn on"}', (error) =>
  {
    if (!error)
    {
      res.status(200).json(createResponse("Ok"));
    }
    else
    {
      returnError("Error saving file " + JSON.stringify(error));
    }
  })
  
};