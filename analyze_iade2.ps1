# kasalar.txt'den documentType=3 (iade) olan satirlarin total'ini bul
$content = Get-Content "Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt" -Raw

# documentType: 3 olan kayitlarin total degerlerini bul
# Pattern: "documentType": 3 ... "total": XXX (ayni obje icinde)
$sales = $content -split '"posDocumentId":'

$normalToplam = 0
$iadeToplam = 0
$normalAdet = 0
$iadeAdet = 0

foreach($sale in $sales) {
    if($sale -match '"documentType":\s*(\d+)') {
        $docType = $Matches[1]
        if($sale -match '"total":\s*([\d.-]+)') {
            $total = [decimal]$Matches[1]
            if($docType -eq "3") {
                $iadeToplam += $total
                $iadeAdet++
            } else {
                $normalToplam += $total
                $normalAdet++
            }
        }
    }
}

Write-Host "=== SONUCLAR ==="
Write-Host "Normal Satis: $normalAdet adet, $($normalToplam.ToString('N2')) TL"
Write-Host "Iade (docType=3): $iadeAdet adet, $($iadeToplam.ToString('N2')) TL"
Write-Host ""
Write-Host "Toplam: $(($normalToplam + $iadeToplam).ToString('N2')) TL"
Write-Host "Normal - Iade: $(($normalToplam - $iadeToplam).ToString('N2')) TL"
