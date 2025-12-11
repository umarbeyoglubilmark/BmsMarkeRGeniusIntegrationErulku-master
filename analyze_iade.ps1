# kasalar.txt'den totalPrice toplamini hesapla - iade dahil/haric
$content = Get-Content "Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt" -Raw

# Tum satirlarin totalPrice toplami
$pattern = '"totalPrice":\s*([\d.-]+)'
$matches = [regex]::Matches($content, $pattern)

$toplamTotalPrice = 0
foreach($m in $matches) {
    $toplamTotalPrice += [decimal]$m.Groups[1].Value
}

# posCode=4 (iade kasasi) satirlarini bul
# Once tum sale bloklarini ayir ve posCode=4 olanlarin total degerlerini topla
$iadePattern = '"posCode":\s*"4"[^}]*?"total":\s*([\d.-]+)'
$iadeMatches = [regex]::Matches($content, $iadePattern)

$iadeToplam = 0
foreach($m in $iadeMatches) {
    $iadeToplam += [decimal]$m.Groups[1].Value
}

# Ayrica documentType=3 (iade) olanlari da kontrol et
$docType3Pattern = '"documentType":\s*3[^}]*?"total":\s*([\d.-]+)'
$docType3Matches = [regex]::Matches($content, $docType3Pattern)

# total alani uzerinden iade hesapla
$totalPattern = '"total":\s*([\d.-]+)'
$totalMatches = [regex]::Matches($content, $totalPattern)

$toplamTotal = 0
foreach($m in $totalMatches) {
    $toplamTotal += [decimal]$m.Groups[1].Value
}

Write-Host "=== SONUCLAR ==="
Write-Host "TotalPrice Toplami: $($toplamTotalPrice.ToString('N2')) TL"
Write-Host "Total Toplami: $($toplamTotal.ToString('N2')) TL"
Write-Host ""
Write-Host "Kasa 4 (Iade) Toplami: $($iadeToplam.ToString('N2')) TL"
Write-Host ""
Write-Host "TotalPrice - Iade: $(($toplamTotalPrice - $iadeToplam).ToString('N2')) TL"
Write-Host "Total - Iade: $(($toplamTotal - $iadeToplam).ToString('N2')) TL"
