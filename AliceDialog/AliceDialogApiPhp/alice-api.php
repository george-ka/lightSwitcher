<?php

date_default_timezone_set('UTC');
header('Content-Type: text/html; charset=utf-8');

function sendInvalidRequest($message)
{
    http_response_code(400);
    print $message;
}

if ($_SERVER['REQUEST_METHOD'] != 'POST')
{
    sendInvalidRequest("Only POST accepted");
    exit;
}

if ($_SERVER["CONTENT_TYPE"] != 'application/json')
{
    sendInvalidRequest("Expected application/json request");
    exit;
}

function createResponse($sayWhat, $endSession)
{
    return array("response" => 
                array("text" => $sayWhat,
                "end_session" => $endSession),
            "version" => "1.0");
}

$json = file_get_contents('php://input');
file_put_contents('logs/request_log_'. date('Ydm-H') .'.txt',  date(DATE_ATOM) . $json . PHP_EOL);

$data = json_decode($json);
$outputCommand = null;

if ($data == null || $data -> request == null )
{
    sendInvalidRequest("Invalid request");
    exit;
}

switch ($data -> request -> command)
{
    case '':
        echo json_encode(createResponse("Слушаю вашу команду", false));
        exit;
    
    case 'включи свет':
        $outputCommand = '0'; 
        break;
    
    case 'выключи свет':
        $outputCommand = '8';
        break;
}


$fp = fopen('command.txt', 'w');
fwrite($fp, $outputCommand);
fclose($fp);

echo json_encode(createResponse("ok", true));
?>