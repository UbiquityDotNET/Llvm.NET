import { Kaleidoscope } from './Kaleidoscope.js'

export default
{
  configureHljs (hljs)
  {
    hljs.registerLanguage('Kaleidoscope', Kaleidoscope);
  },
}
