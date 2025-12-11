$content = Get-Content "Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt" -Raw
$posPattern = '"posCode": "(\d+)"'
$posMatches = [regex]::Matches($content, $posPattern)

$posCounts = @{}
$posTotals = @{}

# posCode ve total'lari birlikte bul
$docPattern = '"posCode": "(\d+)".*?"total": ([\d.-]+)'
$json = $content -replace '\r?\n', ' '

# Her posDocumentId icin posCode ve total'i bul
$docPattern2 = '"posDocumentId":\s*(\d+).*?"total":\s*([\d.-]+).*?"posCode":\s*"(\d+)"'
$docs = [regex]::Matches($json, $docPattern2)

foreach($doc in $docs) {
    $posCode = $doc.Groups[3].Value
    $total = [decimal]$doc.Groups[2].Value

    if(-not $posCounts.ContainsKey($posCode)) {
        $posCounts[$posCode] = 0
        $posTotals[$posCode] = 0
    }
    $posCounts[$posCode]++
    $posTotals[$posCode] += $total
}

Write-Host "Kasa Bazinda Analiz:"
Write-Host "===================="
foreach($pos in $posCounts.Keys | Sort-Object) {
    Write-Host "Kasa $pos : $($posCounts[$pos]) adet, $($posTotals[$pos].ToString('N2')) TL"
}
