module.exports = {
    testEnvironment: 'node',
    testMatch: ['**/tests/**/*.test.js'],
    coverageDirectory: './coverage',
    collectCoverageFrom: ['./tests/*.js'],
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
    setupFilesAfterEnv: ['./tests/setup.js']
  };