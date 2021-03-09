<?php
date_default_timezone_set('UTC');
$fileName = 'command.txt';

if (!file_exists($fileName))
{
    exit;
}

$handle = @fopen($fileName, "r");
$command = fgets($handle);
print $command;
fclose($handle);
unlink($fileName);

?>