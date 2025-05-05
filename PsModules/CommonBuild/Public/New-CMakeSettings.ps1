function New-CmakeSettings( [Parameter(Mandatory, ValueFromPipeline)][CMakeConfig] $configuration )
{
    BEGIN
    {
        $convertedSettings = [System.Collections.Generic.List[hashtable]]::new( )
    }
    PROCESS
    {
        $convertedSettings.Add( $configuration.ToCMakeSettingsJsonifiable( ) )
    }
    END
    {
        ConvertTo-Json -Depth 4 @{ configurations = $convertedSettings }
    }
}

