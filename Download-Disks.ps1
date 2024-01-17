#!/usr/bin/env -S pwsh -NoProfile

$disks = @{
    "2088/Disc999-EliteMasterAndTubeEnhanced.ssd" = "elite.ssd";
    "25/Disc002-ChuckieEgg.ssd" = "chuckie-egg.ssd"
};

New-Item ./disks -ItemType Directory -ErrorAction SilentlyContinue | Out-Null

foreach($key in $disks.Keys) {
    $url = "https://bbcmicro.co.uk/gameimg/discs/$key"
    $file = "disks/$($disks[$key])"
    Write-Host "Downloading $url to $file"
    Invoke-WebRequest -Uri $url -OutFile $file
}