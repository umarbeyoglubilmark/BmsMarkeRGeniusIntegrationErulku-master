$content = Get-Content 'Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt' -Raw

# Dosyadaki tum "documentType": 1 ve "documentType": 3 sayisini bul
$docType1Count = ([regex]::Matches($content, '"documentType":\s*1,')).Count
$docType3Count = ([regex]::Matches($content, '"documentType":\s*3,')).Count

Write-Host "Dosyada bulunan:"
Write-Host "documentType=1 (Satis): $docType1Count adet"
Write-Host "documentType=3 (Iade): $docType3Count adet"
Write-Host ""

# Farkli yaklasim - dosyayi satirlara bolup isle
$lines = $content -split "`n"

$currentDocType = 0
$inLines = $false
$satisTotalPrice = 0
$iadeTotalPrice = 0
$satisKayit = 0
$iadeKayit = 0

foreach($line in $lines) {
    if($line -match '"documentType":\s*(\d+)') {
        $currentDocType = [int]$matches[1]
        if($currentDocType -eq 1) { $satisKayit++ }
        elseif($currentDocType -eq 3) { $iadeKayit++ }
    }

    if($line -match '"totalPrice":\s*([\d.-]+)') {
        $value = [decimal]$matches[1]
        if($currentDocType -eq 1) {
            $satisTotalPrice += $value
        }
        elseif($currentDocType -eq 3) {
            $iadeTotalPrice += $value
        }
    }
}

Write-Host "=== HESAPLAMA SONUCLARI ==="
Write-Host ""
Write-Host "SATIS (documentType=1):"
Write-Host "  Kayit sayisi: $satisKayit"
Write-Host "  Brut Toplam (totalPrice): $($satisTotalPrice.ToString('N2')) TL"
Write-Host ""
Write-Host "IADE (documentType=3):"
Write-Host "  Kayit sayisi: $iadeKayit"
Write-Host "  Brut Toplam (totalPrice): $($iadeTotalPrice.ToString('N2')) TL"
Write-Host ""
Write-Host "NET BRUT: $(($satisTotalPrice - $iadeTotalPrice).ToString('N2')) TL"
Write-Host ""
Write-Host "=== HEDEF KARSILASTIRMA ==="
Write-Host "Hedef: 422.452,00 TL"
Write-Host "Satis Brut: $($satisTotalPrice.ToString('N2')) TL"
Write-Host "Fark: $(($satisTotalPrice - 422452).ToString('N2')) TL"
