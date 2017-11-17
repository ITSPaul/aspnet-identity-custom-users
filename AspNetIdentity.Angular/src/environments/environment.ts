// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  apiHost: '//localhost:50962/',
  jwtTokenConfig: {
    client_id: 'f079d057929d4b3ab74eda26682ebf3c'
  }
};
