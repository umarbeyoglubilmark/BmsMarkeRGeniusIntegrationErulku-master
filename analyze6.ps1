# API total vs sum(lines.totalPrice) karsilastirmasi
$content = Get-Content "Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt" -Raw

# Tum lines icindeki totalPrice toplami
$linePattern = '"totalPrice":\s*([\d.-]+)'
$lineMatches = [regex]::Matches($content, $linePattern)

$linesTotalSum = 0
foreach($lm in $lineMatches) {
    $linesTotalSum += [decimal]$lm.Groups[1].Value
}

# Tum sale total toplami
$totalPattern = '"total":\s*([\d.-]+)'
$totalMatches = [regex]::Matches($content, $totalPattern)

$salesTotalSum = 0
foreach($tm in $totalMatches) {
    $salesTotalSum += [decimal]$tm.Groups[1].Value
}

Write-Host "Lines TotalPrice Toplami: $($linesTotalSum.ToString('N2')) TL"
Write-Host "Sales Total Toplami: $($salesTotalSum.ToString('N2')) TL"
Write-Host "Fark: $(($salesTotalSum - $linesTotalSum).ToString('N2')) TL"
