# te-localhost

SSL enabled localhost environment for TapEnd projects.

## The why?

Provide a convinient and consistent way to run TapEnd projects with SSL enabled in both  local and containerized environmnets.

CI provides SSL using Let's Encrypt in production.

## Installing the certificates

1. Trust the Certificate Authority root

	```bash
		sudo security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain certificate-authority/tapend-ca.pem
	```
	
2. Copy the files in ./localhost-ssl to ~/.localhost-ssl

```bash
	mkdir ~/.localhost-ssl
	cp -r ./localhost-ssl/* ~/.localhost-ssl
```

## Creating the root certificates

1. Create the root certificate
	
	```bash
		openssl genrsa -des3 -out certificate-authority/tapend-ca.key 2048
	```

2. Generate a new Root SSL certificate

	```bash
		openssl req -x509 -new -nodes -key certificate-authority/tapend-ca.key -sha256 -days 1024 -out certificate-authority/tapend-ca.pem
	```

3. Install and trust the root certificate

	```bash
		sudo security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain certificate-authority/tapend-ca.pem
	```

4. Create the certificate key and CSR (Certificate Signing Request) for localhost and 127.0.0.1 environments

	```bash
		openssl req -new -sha256 -nodes -out localhost-ssl/te-localhost.csr -newkey rsa:2048 -keyout localhost-ssl/te-localhost.key -config <( cat certificate.csr.config )
	```

5. Generate a certificate using the key and CSR

	```bash
		openssl x509 -req -in localhost-ssl/te-localhost.csr -CA certificate-authority/tapend-ca.pem -CAkey certificate-authority/tapend-ca.key -CAcreateserial -out localhost-ssl/te-localhost.crt -days 500 -sha256 -extfile v3.ext
	```

6. Generate a pfx file for DotNet Core

	```bash
		openssl pkcs12 -export -out localhost-ssl/te-localhost.pfx -inkey localhost-ssl/te-localhost.key -in localhost-ssl/te-localhost.crt	
	```
