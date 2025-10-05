# PowerShell script to convert SVGs to PNGs
# This script requires Inkscape to be installed

$svgPath = "MarketAlly.Dialogs.Maui\Resources\Images"
$svgFiles = Get-ChildItem -Path $svgPath -Filter "*.svg"

foreach ($svg in $svgFiles) {
    $pngName = $svg.BaseName + ".png"
    $pngPath = Join-Path $svgPath $pngName

    Write-Host "Converting $($svg.Name) to $pngName..."

    # Using Inkscape to convert SVG to PNG at 48x48 resolution
    # You may need to adjust the path to inkscape.exe
    & "C:\Program Files\Inkscape\bin\inkscape.exe" `
        --export-type=png `
        --export-filename=$pngPath `
        --export-width=48 `
        --export-height=48 `
        $svg.FullName
}

Write-Host "Conversion complete!"