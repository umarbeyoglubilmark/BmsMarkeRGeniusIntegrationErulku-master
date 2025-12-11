# kasalar.txt'den totalPrice toplamini hesapla
$content = Get-Content "Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt" -Raw

$pattern = '"totalPrice":\s*([\d.-]+)'
$matches = [regex]::Matches($content, $pattern)

$sum = 0
foreach($m in $matches) {
    $sum += [decimal]$m.Groups[1].Value
}

Write-Host "TotalPrice Toplami: $($sum.ToString('N2')) TL"
Write-Host "Kayit Sayisi: $($matches.Count)"
