module.exports = {
    testEnvironment: 'node',
    testMatch: ['**/tests/**/*.test.ts'],
    coverageDirectory: './coverage',
    collectCoverageFrom: ['./tests/*.ts'],
    transform: {
        '^.+\\.ts$': 'ts-jest'
    },
    coveragePathIgnorePatterns: [
      '/node_modules/',
      '/tests/',
      '/index.ts',
      '/db/types.ts',
      '.*\\.d\\.ts$'
    ],
    setupFilesAfterEnv: ['./tests/setup.ts']
  };