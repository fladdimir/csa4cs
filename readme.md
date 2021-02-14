# csa4cs

Draft for a [SimSharp](https://github.com/heal-research/SimSharp)-based port of [Casymda](https://github.com/fladdimir/casymda).

## development

dotnet 5.0 sdk

### sonarqube

docker-compose up

change pw to admin:a

dotnet tool install --global dotnet-sonarscanner

#### scanner incl. coverage

dotnet-sonarscanner begin /k:"csa4cs" /d:"sonar.login=admin" /d:"sonar.password=a" /d:sonar.cs.opencover.reportsPaths="TestResults/**" /d:sonar.coverage.exclusions="**/Tests/**"

dotnet build .

rm -rf ./TestResults

dotnet test -r TestResults --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover

dotnet-sonarscanner end /d:"sonar.login=admin" /d:"sonar.password=a"
