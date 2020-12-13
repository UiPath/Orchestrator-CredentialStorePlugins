# About

This plugin allows using Hashicorp Vault as a credential store with UiPath Orchestrator.

Supported Auth Methods:
- AppRole (recommeded)
- UsernamePassword
- Ldap
- ClientCertificate
- Token

Supported Secrets Engines:
- KeyValueV1
- KeyValueV2
- ActiveDirectory (read only)
- Cubbyhole

## Starting Vault

You can start Vault in development mode with the command:

```
docker run --rm --cap-add=IPC_LOCK -e VAULT_ADDR=http://localhost:8200 -p 8200:8200 -d --name=dev-vault vault
```

This will start a docker container with Vault, accessible from the host at [`http://localhost:8200`](http://localhost:8200).

You can then check the logs with:

```
docker logs dev-vault
```

You should see something similar to:

```
==> Vault server configuration:

             Api Address: http://0.0.0.0:8200
                     Cgo: disabled
         Cluster Address: https://0.0.0.0:8201
              Go Version: go1.14.7
              Listener 1: tcp (addr: "0.0.0.0:8200", cluster address: "0.0.0.0:8201", max_request_duration: "1m30s", max_request_size: "33554432", tls: "disabled")
               Log Level: info
                   Mlock: supported: true, enabled: false
           Recovery Mode: false
                 Storage: inmem
                 Version: Vault v1.5.4
             Version Sha: 1a730771ec70149293efe91e1d283b10d255c6d1

WARNING! dev mode is enabled! In this mode, Vault runs entirely in-memory
and starts unsealed with a single unseal key. The root token is already
authenticated to the CLI, so you can immediately begin using Vault.

You may need to set the following environment variable:

    $ export VAULT_ADDR='http://0.0.0.0:8200'

The unseal key and root token are displayed below in case you want to
seal/unseal the Vault or re-authenticate.

Unseal Key: H94irEVFFOYMLVwrFyvkV+0Q4fa/eHmxycwzJ9DNMXY=
Root Token: s.hA7RJ5lBqSnKUPd8nrQBaK1f

Development mode should NOT be used in production installations!

==> Vault server started! Log data will stream in below:
```

Several convenience configurations are made when running in dev mode, different than how you would run Vault in production:

- Vault is already initialized with one key share (whereas in normal mode this has to be done explicitly and the number of key shares is 5 by default)
- the unseal key and the root token are displayed in the logs (please write down the root token, we will need it in the following step)
- Vault is [unsealed](https://www.vaultproject.io/docs/concepts/seal)
- in-memory storage is used
- TLS is disabled, so you can access it with http and not deal with SSL certificates
- a kv secret engine v2 is mounted at secret/

## Configuring Authentication

In order to start creating and reading secrets, we must configure an authentication method.

First, we open a shell inside the container:
```
docker exec -it dev-vault sh
```

Next, we will need to authenticate ourselves as root, so we can configure the Vault. We will need the root token that was displayed in the logs we inspected earlier, and we will set an evironment variable with it:
```
export VAULT_TOKEN=s.hA7RJ5lBqSnKUPd8nrQBaK1f
```

We can then check the Vault status with the command `vault status`:
```
vault status
```

Then, we can add a dummy secret for Orchestrator in the KV store:
```
vault kv put secret/applications/orchestrator/testSecret supersecretpassword=123456
```

We have now created the path `secret/applications/orchestrator`, so we must give access to Orchestrator to it. For this, we will first create a [policy](https://www.vaultproject.io/docs/concepts/policies) for reading and writing to this path and all its subpaths:
```
cat <<EOF | vault policy write orchestrator-policy -
path "secret/data/applications/orchestrator/*" {
  capabilities = ["create", "read", "update", "delete"]
}
EOF
```

> **_NOTE:_**  When using a kv secret engine version 2, secrets are written and fetched at path `<mount>/data/<secret-path>` as opposed to `<mount>/<secret-path>` in a kv secret engine version 1. It does not change any of the CLI commands (i.e. you do not specify data in your path). However it does change the policies, since capabilities are applied to the real path. In the example above, the path is `secret/data/applications/orchestrator/*` since we are working with a kv secret engine version 2. It would be `secret/applications/orchestrator/*` with a kv secret engine version 1.

Finally, we enable authentication using [`userpass`](https://www.vaultproject.io/docs/auth/userpass) [auth method](https://www.vaultproject.io/docs/auth), then create a user for Orchestrator and assign the previously created policy:
 ```
vault auth enable userpass
vault write auth/userpass/users/orchestrator password=1qazXSW@ policies=orchestrator-policy
```

> **_NOTE:_**  Orchestrator supports multiple authentication modes. See the [Vault docs](https://www.vaultproject.io/docs/auth/userpass) for how to configure them.

To check that we configured everything correctly, we can log in and try reading the secret we created earlier:
```
vault login -method=userpass username=orchestrator password=1qazXSW@
```
This will output something like:
```
WARNING! The VAULT_TOKEN environment variable is set! This takes precedence
over the value set by this command. To use the value set by this command,
unset the VAULT_TOKEN environment variable or set it to the token displayed
below.

Success! You are now authenticated. The token information displayed below
is already stored in the token helper. You do NOT need to run "vault login"
again. Future Vault requests will automatically use this token.

Key                    Value
---                    -----
token                  s.nwombWQH3gGPDhJumRzxKqgI
token_accessor         aGJL6Pzc6fRRuP8d8tTjS2Kj
token_duration         768h
token_renewable        true
token_policies         ["default" "orchestrator-policy"]
identity_policies      []
policies               ["default" "orchestrator-policy"]
token_meta_username    orchestrator
```

We must now take this token and set it instead of the root token, then try to read the test secret:
```
export VAULT_TOKEN=s.nwombWQH3gGPDhJumRzxKqgI
vault kv get secret/applications/orchestrator/testSecret
```

You should see:
```
====== Metadata ======
Key              Value
---              -----
created_time     2020-10-12T06:24:41.7827631Z
deletion_time    n/a
destroyed        false
version          1

=========== Data ===========
Key                    Value
---                    -----
supersecretpassword    123456
```

> **_NOTE:_**  You can just as well enable [appRole](https://www.vaultproject.io/docs/auth/approle) Orchestrator:
> ```
> / # vault auth enable approle
> / # vault write auth/approle/role/orchestrator policies=orchestrator-policy
> / # vault read auth/approle/role/orchestrator/role-id
> / # vault write -f auth/approle/role/orchestrator/secret-id
> ```
> You will now have a role-id and secret-id for configuring in Orchestrator

## Connect Orchestrator to Vault

### Configure the HashicorpVault plugin

You must copy the assemblies `UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault.dll` and `VaultSharp.dll` to the folder `/plugins` from the Orchestrator installation path. Then edit the file `UiPath.Orchestrator.dll.config`, locate the key `Plugins.SecureStores` and in the value, add the name of the first dll.

The last step is to restart Orchestrator. You should now see the new Credential Store type `HashicorpVault`.

### Configuration options


| Parameter            | Description                                                                                                                                                                                                                                                                                                                             | Example                                |
| -------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------- |
| `VaultUri`           | (Required) The uri to the http api of Hashicorp Vault                                                                                                                                                                                                                                                                                   | `https://myvault.example.com:8200`     |
| `AuthenticationType` | (Required) The authentication method. Possible values are: [`AppRole`](https://www.vaultproject.io/docs/auth/approle), [`UsernamePassword`](https://www.vaultproject.io/docs/auth/userpass), [`Ldap`](https://www.vaultproject.io/docs/auth/ldap), [`Token`](https://www.vaultproject.io/docs/auth/token).                              | `AppRole`                              |
| `RoleId`             | The role id to use with the `AppRole` authentication type.                                                                                                                                                                                                                                                                              | `38bbd8cc-c518-6585-9001-c35da4a9c7b0` |
| `SecretId`           | The secret id to use with the `AppRole` authentication type.                                                                                                                                                                                                                                                                            | `2df402cf-068b-70fd-1081-4761bd475b74` |
| `Username`           | The username to use with `UsernamePassword` or `Ldap` authentication types.                                                                                                                                                                                                                                                             | `myusername`                           |
| `Password`           | The username to use with `UsernamePassword` or `Ldap` authentication types.                                                                                                                                                                                                                                                             | `mypassword`                           |
| `Token`              | The token to use with the `Token` authentication type.                                                                                                                                                                                                                                                                                  | `s.KyEND6rhMOtabxDHNApaxiUY`           |
| `SecretsEngine`      | (Required) The secrets engine to use. Possible values are [`KeyValueV1`](https://www.vaultproject.io/docs/secrets/kv/kv-v1), [`KeyValueV2`](https://www.vaultproject.io/docs/secrets/kv/kv-v2), [`ActiveDirectory`](https://www.vaultproject.io/docs/secrets/ad) and [`Cubbyhole`](https://www.vaultproject.io/docs/secrets/cubbyhole). | `KeyValueV2`                           |
| `SecretsEnginePath`  | The [path of the secrets engine](https://www.vaultproject.io/docs/secrets#secrets-engines-lifecycle). If not supplied, defaults to `kv` for `KeyValueV1`, `kv-v2` for `KeyValueV2` and `ad` for `ActiveDirectory`.                                                                                                                      | `secret`                               |
| `DataPath`           | The path prefix to use for all stored secrets.                                                                                                                                                                                                                                                                                          | `applications/orchestrator`            |
| `Namespace`          | The [namespace](https://www.vaultproject.io/docs/enterprise/namespaces) to use. Only available in Hashicorp Vault Enterprise.                                                                                                                                                                                                           | `orchestrator`                         |

     