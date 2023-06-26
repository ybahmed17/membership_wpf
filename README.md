### ICNE MEMBERSHIP APPLICATION

## CONFIG
* App.config

Add App.config uder `\MemberDesktop\`

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<connectionStrings>
		<add name="connectionStringTEST"
			 connectionString="Server=localhost;userid=root;password={pw};Database=test_icne"
			 providerName="MySql.Data.MySqlClient" />
		<add name="connectionStringPROD"
			connectionString="Server=localhost;userid=root;password={pw};Database=icne"
			providerName="MySql.Data.MySqlClient" />
	</connectionStrings>
	<appSettings>
		<add key="ADMIN" value="3"/>
	</appSettings>
</configuration>
```

Update `password` in `connectionString`

`ADMIN` corresponds current Membership Committee Chair ID in `ADMIN` table in database.
