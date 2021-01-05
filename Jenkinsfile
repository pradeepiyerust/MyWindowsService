pipeline {
    agent any
    stages {
        stage ('Checkout') {
            steps {
 	            checkout([$class: 'GitSCM', branches: [[name: '*/master']], userRemoteConfigs: [[credentialsId: 'MyGitHub', url: 'https://github.com/pradeepiyerust/MyWindowsService.git']]]) 
            }
	    }
        stage ('Build & Install') {
            steps {
                bat "\"${tool 'MSBuild'}\\msbuild\" deploy.proj /p:Configuration=Release"
            }
        }
    }
}